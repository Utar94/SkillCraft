using AutoMapper;
using Logitar;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Aspects;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Characters.Payloads;
using SkillCraft.Core.Languages;
using SkillCraft.Core.Races;

namespace SkillCraft.Core.Characters.Mutations
{
  internal class SaveCharacterStep2MutationHandler : IRequestHandler<SaveCharacterStep2Mutation, CharacterModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public SaveCharacterStep2MutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<CharacterModel> Handle(SaveCharacterStep2Mutation request, CancellationToken cancellationToken)
    {
      SaveCharacterStep2Payload payload = request.Payload;

      Character? character;
      if (request.Id.HasValue)
      {
        character = await _dbContext.Characters
          .SingleOrDefaultAsync(x => x.Uuid == request.Id.Value, cancellationToken)
          ?? throw new EntityNotFoundException<Character>(request.Id.Value);

        if (character.WorldId != _appContext.World.Id || character.Creation?.Step == null)
        {
          throw new UnauthorizedOperationException<Character>(character, _appContext.UserId, _appContext.World);
        }
      }
      else
      {
        character = new Character(_appContext.UserId, _appContext.World);
        _dbContext.Characters.Add(character);
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

      UpdateBonuses(character, payload);
      UpdateCharacterCreation(character, payload);

      await UpdateLanguagesAsync(character, payload, cancellationToken);

      character.Creation!.Step = 2;

      await _dbContext.SaveChangesAsync(cancellationToken);

      _appContext.SetEntity(character);

      return _mapper.Map<CharacterModel>(character);
    }

    private static void UpdateBonuses(Character character, SaveCharacterStep2Payload payload)
    {
      Dictionary<Guid, BonusBase> bonuses = character.Bonuses.ToDictionary(x => x.Id, x => x);

      character.Bonuses.Clear();

      if (payload.Bonuses != null)
      {
        var missingIds = new List<Guid>(capacity: payload.Bonuses.Count());

        foreach (BonusPayload bonusPayload in payload.Bonuses)
        {
          BonusBase? bonus;
          if (bonusPayload.Id.HasValue)
          {
            if (!bonuses.TryGetValue(bonusPayload.Id.Value, out bonus))
            {
              missingIds.Add(bonusPayload.Id.Value);

              continue;
            }
          }
          else
          {
            if (bonusPayload.Type.HasValue)
            {
              bonus = bonusPayload.Type.Value switch
              {
                BonusType.Attribute => new AttributeBonus(Enum.Parse<Attribute>(bonusPayload.Target!)),
                BonusType.Other => new OtherBonus(Enum.Parse<OtherBonusTarget>(bonusPayload.Target!)),
                BonusType.Skill => new SkillBonus(Enum.Parse<Skill>(bonusPayload.Target!)),
                BonusType.Statistic => new StatisticBonus(Enum.Parse<Statistic>(bonusPayload.Target!)),
                _ => throw new InvalidOperationException($"The bonus type \"{bonusPayload.Type.Value}\" is not valid."),
              };
            }
            else
            {
              throw new InvalidOperationException("The bonus type is required.");
            }
          }

          bonus.Description = bonusPayload.Description?.CleanTrim();
          bonus.Permanent = bonusPayload.Permanent;
          bonus.Value = bonusPayload.Value;

          character.Bonuses.Add(bonus);
        }

        if (missingIds.Any())
        {
          throw new CharacterBonusesNotFoundException(missingIds);
        }
      }
    }

    private static void UpdateCharacterCreation(Character character, SaveCharacterStep2Payload payload)
    {
      character.Creation ??= new();
      character.Creation.AttributeBases ??= new();

      character.Creation.AttributeBases.Agility = payload.Creation.AttributeBases.Agility;
      character.Creation.AttributeBases.Coordination = payload.Creation.AttributeBases.Coordination;
      character.Creation.AttributeBases.Intellect = payload.Creation.AttributeBases.Intellect;
      character.Creation.AttributeBases.Mind = payload.Creation.AttributeBases.Mind;
      character.Creation.AttributeBases.Presence = payload.Creation.AttributeBases.Presence;
      character.Creation.AttributeBases.Sensitivity = payload.Creation.AttributeBases.Sensitivity;
      character.Creation.AttributeBases.Vigor = payload.Creation.AttributeBases.Vigor;

      character.Creation.BestAttribute = payload.Creation.BestAttribute;
      character.Creation.WorstAttribute = payload.Creation.WorstAttribute;
      character.Creation.MandatoryAttribute1 = payload.Creation.MandatoryAttribute1;
      character.Creation.MandatoryAttribute2 = payload.Creation.MandatoryAttribute2;
      character.Creation.OptionalAttribute1 = payload.Creation.OptionalAttribute1;
      character.Creation.OptionalAttribute2 = payload.Creation.OptionalAttribute2;
    }

    private async Task UpdateLanguagesAsync(Character character, SaveCharacterStep2Payload payload, CancellationToken cancellationToken)
    {
      character.Languages.Clear();

      if (payload.LanguageIds != null)
      {
        Dictionary<Guid, Language> languages = await _dbContext.Languages
          .Where(x => payload.LanguageIds.Contains(x.Uuid))
          .ToDictionaryAsync(x => x.Uuid, x => x, cancellationToken);

        var missingIds = new List<Guid>(capacity: payload.LanguageIds.Count());

        foreach (Guid languageId in payload.LanguageIds)
        {
          if (!languages.TryGetValue(languageId, out Language? language))
          {
            missingIds.Add(languageId);
          }
          else if (language.WorldId != character.WorldId)
          {
            throw new UnauthorizedOperationException<Language>(language, _appContext.UserId, _appContext.World);
          }
          else
          {
            character.Languages.Add(language);
          }
        }

        if (missingIds.Any())
        {
          throw new LanguagesNotFoundException(missingIds);
        }
      }
    }
  }
}
