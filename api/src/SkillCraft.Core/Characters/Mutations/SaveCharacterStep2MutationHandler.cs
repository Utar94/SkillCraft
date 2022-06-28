using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Aspects;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Characters.Payloads;
using SkillCraft.Core.Races;
using SkillCraft.Core.Repositories;

namespace SkillCraft.Core.Characters.Mutations
{
  internal class SaveCharacterStep2MutationHandler : IRequestHandler<SaveCharacterStep2Mutation, CharacterModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly ICharacterRepository _characterRepository;
    private readonly ICharacterService _characterService;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public SaveCharacterStep2MutationHandler(
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

    public async Task<CharacterModel> Handle(SaveCharacterStep2Mutation request, CancellationToken cancellationToken)
    {
      SaveCharacterStep2Payload payload = request.Payload;

      Character? character;
      if (request.Id.HasValue)
      {
        character = await _characterRepository
          .GetAsync(request.Id.Value, readOnly: false, cancellationToken)
          ?? throw new EntityNotFoundException<Character>(request.Id.Value);

        if (character.WorldId != _appContext.World.Id || character.Creation?.Step == null)
        {
          throw new UnauthorizedOperationException<Character>(character, _appContext.UserId, _appContext.World);
        }

        character.Update(_appContext.UserId);
      }
      else
      {
        character = new Character(_appContext.UserId, _appContext.World);
      }

      Guid[] aspectIds = new[] { payload.Aspect1Id, payload.Aspect2Id };
      Dictionary<Guid, Aspect> aspects = await _dbContext.Aspects
        .Where(x => aspectIds.Contains(x.Uuid))
        .ToDictionaryAsync(x => x.Uuid, x => x, cancellationToken);

      if (!aspects.TryGetValue(payload.Aspect1Id, out Aspect? aspect1))
      {
        throw new EntityNotFoundException<Aspect>(payload.Aspect1Id, nameof(payload.Aspect1Id));
      }
      else if (aspect1.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Aspect>(aspect1, _appContext.UserId, _appContext.World);
      }

      if (!aspects.TryGetValue(payload.Aspect2Id, out Aspect? aspect2))
      {
        throw new EntityNotFoundException<Aspect>(payload.Aspect2Id, nameof(payload.Aspect2Id));
      }
      else if (aspect2.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Aspect>(aspect2, _appContext.UserId, _appContext.World);
      }

      Race race = await _dbContext.Races
        .SingleOrDefaultAsync(x => x.Uuid == payload.RaceId, cancellationToken)
        ?? throw new EntityNotFoundException<Race>(payload.RaceId, nameof(payload.RaceId));

      if (race.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Race>(race, _appContext.UserId, _appContext.World);
      }

      character.Name = payload.Name.Trim();

      character.Aspect1 = aspect1;
      character.Aspect1Id = aspect1.Id;
      character.Aspect2 = aspect2;
      character.Aspect2Id = aspect2.Id;
      character.Race = race;
      character.RaceId = race.Id;

      character.Stature = payload.Stature;
      character.Weight = payload.Weight;
      character.Age = payload.Age;

      _characterService.UpdateBonuses(character, payload.Bonuses);
      _characterService.UpdateCharacterCreation(character, payload.Creation);

      await _characterService.UpdateLanguagesAsync(character, payload.LanguageIds?.ToHashSet(), cancellationToken);

      character.Creation!.Step = 2;

      await _dbContext.SaveChangesAsync(cancellationToken);

      _appContext.SetEntity(character);

      return _mapper.Map<CharacterModel>(character);
    }
  }
}
