using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Characters.Payloads;
using SkillCraft.Core.Customizations;
using SkillCraft.Core.Natures;

namespace SkillCraft.Core.Characters.Mutations
{
  internal class SaveCharacterStep3MutationHandler : IRequestHandler<SaveCharacterStep3Mutation, CharacterModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public SaveCharacterStep3MutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<CharacterModel> Handle(SaveCharacterStep3Mutation request, CancellationToken cancellationToken)
    {
      SaveCharacterStep3Payload payload = request.Payload;

      Character character = await _dbContext.Characters
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Character>(request.Id);

      if (character.WorldId != _appContext.World.Id || character.Creation?.Step == null)
      {
        throw new UnauthorizedOperationException<Character>(character, _appContext.UserId, _appContext.World);
      }

      Nature nature = await _dbContext.Natures
        .Include(x => x.Feat)
        .SingleOrDefaultAsync(x => x.Uuid == payload.NatureId, cancellationToken)
        ?? throw new EntityNotFoundException<Nature>(payload.NatureId, nameof(payload.NatureId));

      if (nature.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Nature>(nature, _appContext.UserId, _appContext.World);
      }

      character.Nature = nature;
      character.NatureId = nature.Id;

      await UpdateCustomizationsAsync(character, payload, cancellationToken);

      character.Creation.Step = 3;

      await _dbContext.SaveChangesAsync(cancellationToken);

      _appContext.SetEntity(character);

      return _mapper.Map<CharacterModel>(character);
    }

    private async Task UpdateCustomizationsAsync(Character character, SaveCharacterStep3Payload payload, CancellationToken cancellationToken)
    {
      character.Customizations.Clear();

      if (payload.CustomizationIds != null)
      {
        HashSet<Guid> customizationIds = payload.CustomizationIds.ToHashSet();
        if (character.Nature?.Feat != null && customizationIds.Contains(character.Nature.Feat.Uuid))
        {
          throw new NatureFeatUnexpectedException();
        }

        Dictionary<Guid, Customization> customizations = await _dbContext.Customizations
          .Where(x => customizationIds.Contains(x.Uuid))
          .ToDictionaryAsync(x => x.Uuid, x => x, cancellationToken);

        Dictionary<CustomizationType, int> counts = customizations.Values
          .GroupBy(x => x.Type)
          .ToDictionary(x => x.Key, x => x.Count());
        counts.TryGetValue(CustomizationType.Feat, out int feats);
        counts.TryGetValue(CustomizationType.Disability, out int disabilities);
        if (feats != disabilities)
        {
          throw new CustomizationCountMismatchException(feats, disabilities);
        }

        var missingIds = new List<Guid>(capacity: customizationIds.Count);

        foreach (Guid customizationId in customizationIds)
        {
          if (!customizations.TryGetValue(customizationId, out Customization? customization))
          {
            missingIds.Add(customizationId);
          }
          else if (customization.WorldId != character.WorldId)
          {
            throw new UnauthorizedOperationException<Customization>(customization, _appContext.UserId, _appContext.World);
          }
          else
          {
            character.Customizations.Add(customization);
          }
        }

        if (missingIds.Any())
        {
          throw new CustomizationsNotFoundException(missingIds);
        }
      }
    }
  }
}
