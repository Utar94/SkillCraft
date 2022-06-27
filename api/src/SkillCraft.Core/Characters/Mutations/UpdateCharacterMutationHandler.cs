using AutoMapper;
using Logitar;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Characters.Payloads;
using SkillCraft.Core.Conditions;
using SkillCraft.Core.Languages;
using SkillCraft.Core.Powers;
using SkillCraft.Core.Talents;

namespace SkillCraft.Core.Characters.Mutations
{
  internal class UpdateCharacterMutationHandler : IRequestHandler<UpdateCharacterMutation, CharacterModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public UpdateCharacterMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    /// <summary>
    /// TODO(fpion): refactor
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException{Character}"></exception>
    /// <exception cref="UnauthorizedOperationException{Character}"></exception>
    public async Task<CharacterModel> Handle(UpdateCharacterMutation request, CancellationToken cancellationToken)
    {
      UpdateCharacterPayload payload = request.Payload;

      Character character = await _dbContext.Characters
        .Include(x => x.Aspect1)
        .Include(x => x.Aspect2)
        .Include(x => x.Caste)
        .Include(x => x.Education)
        .Include(x => x.Nature)
        .Include(x => x.Race)
        .Include(x => x.Conditions).ThenInclude(x => x.Condition)
        .Include(x => x.Customizations)
        .Include(x => x.Languages)
        .Include(x => x.Powers).ThenInclude(x => x.Power)
        .Include(x => x.Talents).ThenInclude(x => x.Talent).ThenInclude(x => x!.Options)
        .Include(x => x.Talents).ThenInclude(x => x.Option)
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Character>(request.Id);

      if (character.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Character>(character, _appContext.UserId, _appContext.World);
      }

      character.Name = payload.Name.Trim();
      character.Player = payload.Player?.CleanTrim();

      character.Stature = payload.Stature;
      character.Weight = payload.Weight;
      character.Age = payload.Age;

      character.Experience = payload.Experience;
      character.Vitality = payload.Vitality;
      character.Stamina = payload.Stamina;

      character.BloodAlcoholContent = payload.BloodAlcoholContent;
      character.Intoxication = payload.Intoxication;

      character.Description = payload.Description?.CleanTrim();

      UpdateBonuses(character, payload);

      await UpdateConditionsAsync(character, payload, cancellationToken);

      await UpdateLanguagesAsync(character, payload, cancellationToken);

      await UpdatePowersAsync(character, payload, cancellationToken);
      await UpdateTalentsAsync(character, payload, cancellationToken);
      UpdateSkillRanks(character, payload);
      character.Validate();

      character.Update(_appContext.UserId);

      await _dbContext.SaveChangesAsync(cancellationToken);

      _appContext.SetEntity(character);

      return _mapper.Map<CharacterModel>(character);
    }

    /// <summary>
    /// TODO(fpion): refactor
    /// </summary>
    /// <param name="character"></param>
    /// <param name="payload"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="CharacterBonusesNotFoundException"></exception>
    private static void UpdateBonuses(Character character, UpdateCharacterPayload payload)
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
          else if (bonusPayload.Type.HasValue)
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
            throw new InvalidOperationException("The bonus ID or type is required.");
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

    private async Task UpdateConditionsAsync(Character character, UpdateCharacterPayload payload, CancellationToken cancellationToken)
    {
      Dictionary<int, CharacterCondition> characterConditions = character.Conditions
        .Where(x => x.Condition != null)
        .ToDictionary(x => x.ConditionId, x => x);

      character.Conditions.Clear();

      if (payload.Conditions != null)
      {
        HashSet<Guid> conditionIds = payload.Conditions.Select(x => x.ConditionId).ToHashSet();
        Dictionary<Guid, Condition> conditions = await _dbContext.Conditions
          .Where(x => conditionIds.Contains(x.Uuid))
          .ToDictionaryAsync(x => x.Uuid, x => x, cancellationToken);

        var missingIds = new List<Guid>(capacity: conditionIds.Count);

        foreach (CharacterConditionPayload conditionPayload in payload.Conditions)
        {
          if (!conditions.TryGetValue(conditionPayload.ConditionId, out Condition? condition))
          {
            missingIds.Add(conditionPayload.ConditionId);

            continue;
          }
          else if (conditionPayload.Level > condition.MaxLevel)
          {
            throw new ConditionLevelExceededException(condition, conditionPayload.Level);
          }

          if (!characterConditions.TryGetValue(condition.Id, out CharacterCondition? characterCondition))
          {
            characterCondition = new CharacterCondition(character, condition);
          }

          characterCondition.Level = conditionPayload.Level;

          character.Conditions.Add(characterCondition);
        }
      }
    }

    /// <summary>
    /// TODO(fpion): refactor
    /// </summary>
    /// <param name="character"></param>
    /// <param name="payload"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedOperationException{Language}"></exception>
    /// <exception cref="LanguagesNotFoundException"></exception>
    private async Task UpdateLanguagesAsync(Character character, UpdateCharacterPayload payload, CancellationToken cancellationToken)
    {
      character.Languages.Clear();

      if (payload.LanguageIds != null)
      {
        HashSet<Guid> languageIds = payload.LanguageIds.ToHashSet();
        Dictionary<Guid, Language> languages = await _dbContext.Languages
          .Where(x => languageIds.Contains(x.Uuid))
          .ToDictionaryAsync(x => x.Uuid, x => x, cancellationToken);

        var missingIds = new List<Guid>(capacity: languageIds.Count);

        foreach (Guid languageId in languageIds)
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

    /// <summary>
    /// TODO(fpion): refactor
    /// </summary>
    /// <param name="character"></param>
    /// <param name="payload"></param>
    private static void UpdateSkillRanks(Character character, UpdateCharacterPayload payload)
    {
      if (payload.SkillRanks == null)
      {
        character.SkillRanks.Clear();
      }
      else
      {
        HashSet<Guid> ids = payload.SkillRanks
          .Where(x => x.Id.HasValue).Select(x => x.Id!.Value)
          .ToHashSet();
        character.SkillRanks = character.SkillRanks
          .Where(x => ids.Contains(x.Id))
          .ToList();

        Dictionary<Skill, int> newRanks = payload.SkillRanks.Where(x => x.Skill.HasValue)
          .GroupBy(x => x.Skill!.Value)
          .ToDictionary(x => x.Key, x => x.Count());
        HashSet<Skill> trained = character.Talents.Where(x => x.Talent?.Skill.HasValue == true)
          .Select(x => x.Talent!.Skill!.Value)
          .ToHashSet();
        foreach (var (skill, count) in newRanks)
        {
          character.SkillRanks.Add(new SkillRank(skill, trained.Contains(skill)));
        }
      }
    }

    /// <summary>
    /// TODO(fpion): refactor
    /// </summary>
    /// <param name="character"></param>
    /// <param name="payload"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="UnauthorizedOperationException{Power}"></exception>
    /// <exception cref="TalentCostExceededException"></exception>
    private async Task UpdatePowersAsync(Character character, UpdateCharacterPayload payload, CancellationToken cancellationToken)
    {
      Dictionary<Guid, CharacterPower> characterPowers = character.Powers
        .ToDictionary(x => x.Uuid, x => x);

      character.Powers.Clear();

      if (payload.Powers != null)
      {
        var missingIds = new List<Guid>(capacity: payload.Powers.Count());
        var missingPowers = new List<Guid>(capacity: payload.Powers.Count());

        HashSet<Guid> powerIds = payload.Powers.Where(x => x.PowerId.HasValue)
          .Select(x => x.PowerId!.Value)
          .ToHashSet();
        Dictionary<Guid, Power> powers = await _dbContext.Powers
          .Where(x => powerIds.Contains(x.Uuid))
          .ToDictionaryAsync(x => x.Uuid, x => x, cancellationToken);

        foreach (CharacterPowerPayload powerPayload in payload.Powers)
        {
          CharacterPower? characterPower;
          Power? power;
          if (powerPayload.Id.HasValue)
          {
            if (characterPowers.TryGetValue(powerPayload.Id.Value, out characterPower))
            {
              power = characterPower.Power ?? throw new InvalidOperationException($"The {nameof(characterPower.Power)} is required.");
            }
            else
            {
              missingIds.Add(powerPayload.Id.Value);

              continue;
            }
          }
          else if (powerPayload.PowerId.HasValue)
          {
            if (powers.TryGetValue(powerPayload.PowerId.Value, out power))
            {
              if (power.WorldId != _appContext.World.Id)
              {
                throw new UnauthorizedOperationException<Power>(power, _appContext.UserId, _appContext.World);
              }
            }
            else
            {
              missingPowers.Add(powerPayload.PowerId.Value);

              continue;
            }

            characterPower = new CharacterPower(character, power);
          }
          else
          {
            throw new InvalidOperationException("The character power payload is missing an ID.");
          }

          if (powerPayload.Cost > power.Cost)
          {
            throw new TalentCostExceededException(power, powerPayload.Cost);
          }

          characterPower.Cost = powerPayload.Cost;
          characterPower.Description = powerPayload.Description?.CleanTrim();

          character.Powers.Add(characterPower);
        }
      }
    }

    /// <summary>
    /// TODO(fpion): refactor
    /// </summary>
    /// <param name="character"></param>
    /// <param name="payload"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="UnauthorizedOperationException{Talent}"></exception>
    /// <exception cref="TalentOptionRequiredException"></exception>
    /// <exception cref="TalentCostExceededException"></exception>
    private async Task UpdateTalentsAsync(Character character, UpdateCharacterPayload payload, CancellationToken cancellationToken)
    {
      Dictionary<Guid, CharacterTalent> characterTalents = character.Talents
        .ToDictionary(x => x.Uuid, x => x);

      character.Talents.Clear();

      if (payload.Talents != null)
      {
        var missingIds = new List<Guid>(capacity: payload.Talents.Count());
        var missingTalents = new List<Guid>(capacity: payload.Talents.Count());
        var missingOptions = new List<Guid>(capacity: payload.Talents.Count());

        HashSet<Guid> talentIds = payload.Talents.Where(x => x.TalentId.HasValue)
          .Select(x => x.TalentId!.Value)
          .ToHashSet();
        Dictionary<Guid, Talent> talents = await _dbContext.Talents
          .Include(x => x.Options)
          .Where(x => talentIds.Contains(x.Uuid))
          .ToDictionaryAsync(x => x.Uuid, x => x, cancellationToken);

        foreach (CharacterTalentPayload talentPayload in payload.Talents)
        {
          CharacterTalent? characterTalent;
          Talent? talent;
          if (talentPayload.Id.HasValue)
          {
            if (characterTalents.TryGetValue(talentPayload.Id.Value, out characterTalent))
            {
              talent = characterTalent.Talent ?? throw new InvalidOperationException($"The {nameof(characterTalent.Talent)} is required.");
            }
            else
            {
              missingIds.Add(talentPayload.Id.Value);

              continue;
            }
          }
          else if (talentPayload.TalentId.HasValue)
          {
            if (talents.TryGetValue(talentPayload.TalentId.Value, out talent))
            {
              if (talent.WorldId != _appContext.World.Id)
              {
                throw new UnauthorizedOperationException<Talent>(talent, _appContext.UserId, _appContext.World);
              }
            }
            else
            {
              missingTalents.Add(talentPayload.TalentId.Value);

              continue;
            }

            characterTalent = new CharacterTalent(character, talent);
          }
          else
          {
            throw new InvalidOperationException("The character talent payload is missing an ID.");
          }

          if (talent.Options.Any() && talentPayload.OptionId == null)
          {
            throw new TalentOptionRequiredException();
          }

          TalentOption? option = null;
          if (talentPayload.OptionId.HasValue)
          {
            option = talent.Options.SingleOrDefault(x => x.Uuid == talentPayload.OptionId.Value);
            if (option == null)
            {
              missingOptions.Add(talentPayload.OptionId.Value);

              continue;
            }
          }

          if (talentPayload.Cost > talent.Cost)
          {
            throw new TalentCostExceededException(talent, talentPayload.Cost);
          }

          characterTalent.Cost = talentPayload.Cost;
          characterTalent.Description = talentPayload.Description?.CleanTrim();

          characterTalent.Option = option;
          characterTalent.OptionId = option?.Id;

          character.Talents.Add(characterTalent);
        }
      }
    }
  }
}
