using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Characters.Payloads;
using SkillCraft.Core.Natures;
using SkillCraft.Core.Repositories;

namespace SkillCraft.Core.Characters.Mutations
{
  internal class SaveCharacterStep3MutationHandler : IRequestHandler<SaveCharacterStep3Mutation, CharacterModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly ICharacterRepository _characterRepository;
    private readonly ICharacterService _characterService;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public SaveCharacterStep3MutationHandler(
      IApplicationContext appContext,
      ICharacterRepository characterRepository,
      ICharacterService characterService,
      IDbContext dbContext,
      IMapper mapper
    )
    {
      _appContext = appContext;
      _characterRepository = characterRepository;
      _characterService = characterService;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<CharacterModel> Handle(SaveCharacterStep3Mutation request, CancellationToken cancellationToken)
    {
      SaveCharacterStep3Payload payload = request.Payload;

      Character character = await _characterRepository
        .GetAsync(request.Id, readOnly: false, cancellationToken)
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

      await _characterService.UpdateCustomizationsAsync(character, payload.CustomizationIds?.ToHashSet(), cancellationToken);

      character.Creation.Step = 3;

      character.Update(_appContext.UserId);

      await _dbContext.SaveChangesAsync(cancellationToken);

      _appContext.SetEntity(character);

      return _mapper.Map<CharacterModel>(character);
    }
  }
}
