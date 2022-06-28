using Logitar;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Characters.Payloads;
using SkillCraft.Core.Conditions;
using SkillCraft.Core.Customizations;
using SkillCraft.Core.Languages;
using SkillCraft.Core.Powers;
using SkillCraft.Core.Talents;

namespace SkillCraft.Core.Characters
{
  internal class CharacterService : ICharacterService
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;

    public CharacterService(IApplicationContext appContext, IDbContext dbContext)
    {
      _appContext = appContext;
      _dbContext = dbContext;
    }

    public void UpdateBonuses(Character character, IEnumerable<BonusPayload>? payloads)
    {
      ArgumentNullException.ThrowIfNull(character);

      Dictionary<Guid, BonusBase> bonuses = character.Bonuses.ToDictionary(x => x.Id, x => x);

      character.Bonuses.Clear();

      if (payloads != null)
      {
        var missingIds = new List<Guid>(capacity: payloads.Count());

        foreach (BonusPayload payload in payloads)
        {
          BonusBase? bonus;
          if (payload.Id.HasValue)
          {
            if (!bonuses.TryGetValue(payload.Id.Value, out bonus))
            {
              missingIds.Add(payload.Id.Value);

              continue;
            }
          }
          else if (payload.Type.HasValue)
          {
            bonus = payload.Type.Value switch
            {
              BonusType.Attribute => new AttributeBonus(Enum.Parse<Attribute>(payload.Target!)),
              BonusType.Other => new OtherBonus(Enum.Parse<OtherBonusTarget>(payload.Target!)),
              BonusType.Skill => new SkillBonus(Enum.Parse<Skill>(payload.Target!)),
              BonusType.Statistic => new StatisticBonus(Enum.Parse<Statistic>(payload.Target!)),
              _ => throw new InvalidOperationException($"The bonus type \"{payload.Type.Value}\" is not valid."),
            };
          }
          else
          {
            throw new InvalidOperationException("The bonus ID or type is required.");
          }

          bonus.Description = payload.Description?.CleanTrim();
          bonus.Permanent = payload.Permanent;
          bonus.Value = payload.Value;

          character.Bonuses.Add(bonus);
        }

        if (missingIds.Any())
        {
          throw new CharacterBonusesNotFoundException(missingIds);
        }
      }
    }

    public void UpdateCharacterCreation(Character character, CharacterCreationPayload payload)
    {
      ArgumentNullException.ThrowIfNull(character);
      ArgumentNullException.ThrowIfNull(payload);

      character.Creation ??= new();

      character.Creation.AttributeBases[Attribute.Agility] = payload.AttributeBases.Agility;
      character.Creation.AttributeBases[Attribute.Coordination] = payload.AttributeBases.Coordination;
      character.Creation.AttributeBases[Attribute.Intellect] = payload.AttributeBases.Intellect;
      character.Creation.AttributeBases[Attribute.Mind] = payload.AttributeBases.Mind;
      character.Creation.AttributeBases[Attribute.Presence] = payload.AttributeBases.Presence;
      character.Creation.AttributeBases[Attribute.Sensitivity] = payload.AttributeBases.Sensitivity;
      character.Creation.AttributeBases[Attribute.Vigor] = payload.AttributeBases.Vigor;

      character.Creation.BestAttribute = payload.BestAttribute;
      character.Creation.WorstAttribute = payload.WorstAttribute;
      character.Creation.MandatoryAttribute1 = payload.MandatoryAttribute1;
      character.Creation.MandatoryAttribute2 = payload.MandatoryAttribute2;
      character.Creation.OptionalAttribute1 = payload.OptionalAttribute1;
      character.Creation.OptionalAttribute2 = payload.OptionalAttribute2;
    }

    public void UpdateSkillRanks(Character character, IEnumerable<SkillRankPayload>? payloads)
    {
      ArgumentNullException.ThrowIfNull(character);

      if (payloads == null)
      {
        character.SkillRanks.Clear();
      }
      else
      {
        HashSet<Guid> ids = payloads.Where(x => x.Id.HasValue)
          .Select(x => x.Id!.Value)
          .ToHashSet();
        character.SkillRanks = character.SkillRanks
          .Where(x => ids.Contains(x.Id))
          .ToList();

        Dictionary<Skill, int> newRanks = payloads.Where(x => x.Skill.HasValue)
          .GroupBy(x => x.Skill!.Value)
          .ToDictionary(x => x.Key, x => x.Count());
        HashSet<Skill> trained = character.Talents.Where(x => x.Talent?.Skill.HasValue == true)
          .Select(x => x.Talent!.Skill!.Value)
          .ToHashSet();
        foreach (var (skill, count) in newRanks)
        {
          character.SkillRanks.Add(new SkillRank(skill, trained.Contains(skill)));
        }

        character.Validate();
      }
    }

    public async Task UpdateConditionsAsync(Character character, IEnumerable<CharacterConditionPayload>? payloads, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(character);

      Dictionary<int, CharacterCondition> characterConditions = character.Conditions
        .Where(x => x.Condition != null)
        .ToDictionary(x => x.ConditionId, x => x);

      character.Conditions.Clear();

      if (payloads != null)
      {
        HashSet<Guid> conditionIds = payloads.Select(x => x.ConditionId).ToHashSet();
        Dictionary<Guid, Condition> conditions = await _dbContext.Conditions
          .Where(x => conditionIds.Contains(x.Uuid))
          .ToDictionaryAsync(x => x.Uuid, x => x, cancellationToken);

        var missingIds = new List<Guid>(capacity: conditionIds.Count);

        foreach (CharacterConditionPayload payload in payloads)
        {
          if (!conditions.TryGetValue(payload.ConditionId, out Condition? condition))
          {
            missingIds.Add(payload.ConditionId);

            continue;
          }
          else if (condition.WorldId != _appContext.World.Id)
          {
            throw new UnauthorizedOperationException<Condition>(condition, _appContext.UserId, _appContext.World);
          }
          else if (payload.Level > condition.MaxLevel)
          {
            throw new ConditionLevelExceededException(condition, payload.Level);
          }

          if (!characterConditions.TryGetValue(condition.Id, out CharacterCondition? characterCondition))
          {
            characterCondition = new CharacterCondition(character, condition);
          }

          characterCondition.Level = payload.Level;

          character.Conditions.Add(characterCondition);
        }

        if (missingIds.Any())
        {
          throw new ConditionsNotFoundException(missingIds);
        }
      }
    }

    public async Task UpdateCustomizationsAsync(Character character, ISet<Guid>? customizationIds, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(character);

      character.Customizations.Clear();

      if (customizationIds != null)
      {
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

    public async Task UpdateLanguagesAsync(Character character, ISet<Guid>? languageIds, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(character);

      character.Languages.Clear();

      if (languageIds != null)
      {
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

    public async Task UpdatePowersAsync(Character character, IEnumerable<CharacterPowerPayload>? payloads, CancellationToken cancellationToken = default)
    {
      ArgumentNullException.ThrowIfNull(character);

      Dictionary<int, CharacterPower> characterPowers = character.Powers
        .ToDictionary(x => x.PowerId, x => x);

      character.Powers.Clear();

      if (payloads != null)
      {
        var missingIds = new List<Guid>(capacity: payloads.Count());

        HashSet<Guid> powerIds = payloads.Select(x => x.PowerId).ToHashSet();
        Dictionary<Guid, Power> powers = await _dbContext.Powers
          .Where(x => powerIds.Contains(x.Uuid))
          .ToDictionaryAsync(x => x.Uuid, x => x, cancellationToken);

        foreach (CharacterPowerPayload payload in payloads)
        {
          if (powers.TryGetValue(payload.PowerId, out Power? power))
          {
            if (power.WorldId != _appContext.World.Id)
            {
              throw new UnauthorizedOperationException<Power>(power, _appContext.UserId, _appContext.World);
            }
          }
          else
          {
            missingIds.Add(payload.PowerId);

            continue;
          }

          if (payload.Cost > power.Cost)
          {
            throw new TalentCostExceededException(power, payload.Cost);
          }
          if (power.Tier > character.Tier)
          {
            throw new InvalidCharacterPowerTierException(character.Tier, power);
          }

          if (!characterPowers.TryGetValue(power.Id, out CharacterPower? characterPower))
          {
            characterPower = new CharacterPower(character, power);
          }

          characterPower.Cost = payload.Cost;
          characterPower.Description = payload.Description?.CleanTrim();

          character.Powers.Add(characterPower);
        }

        if (missingIds.Any())
        {
          throw new PowersNotFoundException(missingIds);
        }

        character.Validate();
      }
    }

    public async Task UpdateTalentsAsync(Character character, IEnumerable<CharacterTalentPayload>? payloads, CancellationToken cancellationToken = default)
    {
      ArgumentNullException.ThrowIfNull(character);

      Dictionary<Guid, CharacterTalent> characterTalents = character.Talents
        .ToDictionary(x => x.Uuid, x => x);

      character.Talents.Clear();

      if (payloads != null)
      {
        var missingIds = new List<Guid>(capacity: payloads.Count());
        var missingTalents = new List<Guid>(capacity: payloads.Count());
        var missingOptions = new List<Guid>(capacity: payloads.Count());

        HashSet<Guid> talentIds = payloads.Where(x => x.TalentId.HasValue)
          .Select(x => x.TalentId!.Value)
          .ToHashSet();
        Dictionary<Guid, Talent> talents = await _dbContext.Talents
          .Include(x => x.Options)
          .Where(x => talentIds.Contains(x.Uuid))
          .ToDictionaryAsync(x => x.Uuid, x => x, cancellationToken);

        foreach (CharacterTalentPayload payload in payloads)
        {
          CharacterTalent? characterTalent;
          Talent? talent;
          if (payload.Id.HasValue)
          {
            if (characterTalents.TryGetValue(payload.Id.Value, out characterTalent))
            {
              talent = characterTalent.Talent ?? throw new InvalidOperationException($"The {nameof(characterTalent.Talent)} is required.");
            }
            else
            {
              missingIds.Add(payload.Id.Value);

              continue;
            }
          }
          else if (payload.TalentId.HasValue)
          {
            if (talents.TryGetValue(payload.TalentId.Value, out talent))
            {
              if (talent.WorldId != _appContext.World.Id)
              {
                throw new UnauthorizedOperationException<Talent>(talent, _appContext.UserId, _appContext.World);
              }
            }
            else
            {
              missingTalents.Add(payload.TalentId.Value);

              continue;
            }

            characterTalent = new CharacterTalent(character, talent);
          }
          else
          {
            throw new InvalidOperationException("The character talent payload is missing an ID.");
          }

          if (talent.Options.Any() && payload.OptionId == null)
          {
            throw new TalentOptionRequiredException();
          }

          TalentOption? option = null;
          if (payload.OptionId.HasValue)
          {
            option = talent.Options.SingleOrDefault(x => x.Uuid == payload.OptionId.Value);
            if (option == null)
            {
              missingOptions.Add(payload.OptionId.Value);

              continue;
            }
          }

          if (payload.Cost > talent.Cost)
          {
            throw new TalentCostExceededException(talent, payload.Cost);
          }
          if (talent.Tier > character.Tier)
          {
            throw new InvalidCharacterTalentTierException(character.Tier, talent);
          }

          characterTalent.Cost = payload.Cost;
          characterTalent.Description = payload.Description?.CleanTrim();

          characterTalent.Option = option;
          characterTalent.OptionId = option?.Id;

          character.Talents.Add(characterTalent);
        }

        if (missingIds.Any())
        {
          throw new CharacterTalentsNotFoundException(missingIds);
        }
        if (missingTalents.Any())
        {
          throw new TalentsNotFoundException(missingTalents);
        }
        if (missingOptions.Any())
        {
          throw new TalentOptionsNotFoundException(missingOptions);
        }

        character.Validate();
      }
    }
  }
}
