using AutoMapper;
using Logitar;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Languages;
using SkillCraft.Core.Races.Models;
using SkillCraft.Core.Races.Payloads;

namespace SkillCraft.Core.Races.Mutations
{
  internal abstract class SaveRaceHandler
  {
    protected SaveRaceHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      AppContext = appContext;
      DbContext = dbContext;
      Mapper = mapper;
    }

    protected IApplicationContext AppContext { get; }
    protected IDbContext DbContext { get; }
    protected IMapper Mapper { get; }

    protected async Task<RaceModel> ExecuteAsync(Race race, SaveRacePayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(race);
      ArgumentNullException.ThrowIfNull(payload);

      race.Description = payload.Description?.CleanTrim();
      race.Name = payload.Name.Trim();

      race.Size = payload.Size;
      race.StatureRoll = payload.StatureRoll;

      race.ExtraAttributes = payload.ExtraAttributes;
      race.ExtraLanguages = payload.ExtraLanguages;

      UpdateAgeThresholds(race, payload);
      UpdateCharacteristics(race, payload);
      UpdateTexts(race, payload);
      UpdateTraits(race, payload);
      UpdateWeightRolls(race, payload);

      await UpdateLanguagesAsync(race, payload, cancellationToken);

      await DbContext.SaveChangesAsync(cancellationToken);

      AppContext.SetEntity(race);

      return Mapper.Map<RaceModel>(race);
    }

    private static void UpdateAgeThresholds(Race race, SaveRacePayload payload)
    {
      race.AgeThresholds = payload.AgeThresholds == null ? null : new[]
      {
        payload.AgeThresholds.Teenager,
        payload.AgeThresholds.Adult,
        payload.AgeThresholds.Mature,
        payload.AgeThresholds.Venerable
      };
    }

    private static void UpdateCharacteristics(Race race, SaveRacePayload payload)
    {
      if (payload.Attributes == null)
      {
        race.Attributes.Clear();
      }
      else
      {
        race.Attributes = payload.Attributes.ToDictionary(x => x.Attribute, x => x.Bonus);
      }

      if (payload.Names == null)
      {
        race.Names.Clear();
      }
      else
      {
        race.Names = payload.Names.ToDictionary(
          x => x.Category,
          x => x.Values.Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(name => name.Trim())
            .OrderBy(name => name)
            .ToHashSet()
        );
      }

      if (payload.Speeds == null)
      {
        race.Speeds.Clear();
      }
      else
      {
        race.Speeds = payload.Speeds.ToDictionary(x => x.Type, x => x.Value);
      }
    }

    private async Task UpdateLanguagesAsync(Race race, SaveRacePayload payload, CancellationToken cancellationToken)
    {
      race.Languages.Clear();

      if (payload.LanguageIds != null)
      {
        HashSet<Guid> languageIds = payload.LanguageIds.ToHashSet();
        Dictionary<Guid, Language> languages = await DbContext.Languages
          .Where(x => languageIds.Contains(x.Uuid))
          .ToDictionaryAsync(x => x.Uuid, x => x, cancellationToken);

        var missingIds = new List<Guid>(capacity: languageIds.Count);

        foreach (Guid languageId in languageIds)
        {
          if (!languages.TryGetValue(languageId, out Language? language))
          {
            missingIds.Add(languageId);
          }
          else if (language.WorldId != race.WorldId)
          {
            throw new UnauthorizedOperationException<Language>(language, AppContext.UserId, AppContext.World);
          }
          else
          {
            race.Languages.Add(language);
          }
        }

        if (missingIds.Any())
        {
          throw new LanguagesNotFoundException(missingIds);
        }
      }
    }

    private static void UpdateTexts(Race race, SaveRacePayload payload)
    {
      race.AgeText = payload.AgeText?.CleanTrim();
      race.AttributesText = payload.AttributesText?.CleanTrim();
      race.LanguagesText = payload.LanguagesText?.CleanTrim();
      race.NamesText = payload.NamesText?.CleanTrim();
      race.PeopleText = payload.PeopleText?.CleanTrim();
      race.SizeText = payload.SizeText?.CleanTrim();
      race.SpeedText = payload.SpeedText?.CleanTrim();
      race.TraitsText = payload.TraitsText?.CleanTrim();
      race.WeightText = payload.WeightText?.CleanTrim();
    }

    private static void UpdateTraits(Race race, SaveRacePayload payload)
    {
      Dictionary<Guid, RacialTrait> traits = race.Traits.ToDictionary(x => x.Id, x => x);

      race.Traits.Clear();

      if (payload.Traits != null)
      {
        var missingIds = new List<Guid>(capacity: payload.Traits.Count());

        foreach (RacialTraitPayload traitPayload in payload.Traits)
        {
          RacialTrait? racialTrait;
          if (traitPayload.Id.HasValue)
          {
            if (!traits.TryGetValue(traitPayload.Id.Value, out racialTrait))
            {
              missingIds.Add(traitPayload.Id.Value);

              continue;
            }
          }
          else
          {
            racialTrait = new RacialTrait();
          }

          racialTrait.Description = traitPayload.Description?.CleanTrim();
          racialTrait.Name = traitPayload.Name.Trim();

          race.Traits.Add(racialTrait);
        }

        if (missingIds.Any())
        {
          throw new RacialTraitsNotFoundException(missingIds);
        }
      }
    }

    private static void UpdateWeightRolls(Race race, SaveRacePayload payload)
    {
      race.WeightRolls = payload.WeightRolls == null ? null : new[]
      {
        payload.WeightRolls.Skinny,
        payload.WeightRolls.Thin,
        payload.WeightRolls.Normal,
        payload.WeightRolls.Overweight,
        payload.WeightRolls.Obese
      };
    }
  }
}
