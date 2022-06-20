using SkillCraft.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Races.Payloads
{
  public class SaveRacePayload : IValidatableObject
  {
    public string? Description { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    public IEnumerable<AttributeBonusPayload>? Attributes { get; set; }

    public IEnumerable<NameCategoryPayload>? Names { get; set; }

    public IEnumerable<RacialSpeedPayload>? Speeds { get; set; }

    public AgeThresholdsPayload? AgeThresholds { get; set; }

    public SizeCategory Size { get; set; }

    [Roll]
    public string? StatureRoll { get; set; }

    public WeightRollsPayload? WeightRolls { get; set; }

    public IEnumerable<Guid>? LanguageIds { get; set; }

    public IEnumerable<RacialTraitPayload>? Traits { get; set; }

    [Range(0, 3)]
    public int ExtraAttributes { get; set; }

    [Range(0, 3)]
    public int ExtraLanguages { get; set; }

    [StringLength(1000)]
    public string? AgeText { get; set; }

    [StringLength(1000)]
    public string? AttributesText { get; set; }

    [StringLength(1000)]
    public string? LanguagesText { get; set; }

    [StringLength(1000)]
    public string? NamesText { get; set; }

    [StringLength(1000)]
    public string? PeopleText { get; set; }

    [StringLength(1000)]
    public string? SizeText { get; set; }

    [StringLength(1000)]
    public string? SpeedText { get; set; }

    [StringLength(1000)]
    public string? TraitsText { get; set; }

    [StringLength(1000)]
    public string? WeightText { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var results = new List<ValidationResult>(capacity: 3);

      if (Attributes != null)
      {
        IEnumerable<Attribute> attributes = Attributes.GroupBy(x => x.Attribute)
          .Where(x => x.Count() > 1)
          .Select(x => x.Key);
        if (attributes.Any())
        {
          results.Add(new ValidationResult(
            errorMessage: $"Each attribute must only appear once: {string.Join(", ", attributes)}.",
            memberNames: new[] { nameof(Attributes) }
          ));
        }
      }

      if (Names != null)
      {
        IEnumerable<string> categories = Names.GroupBy(x => x.Category)
          .Where(x => x.Count() > 1)
          .Select(x => x.Key);
        if (categories.Any())
        {
          results.Add(new ValidationResult(
            errorMessage: $"Each category must only appear once: {string.Join(", ", categories)}.",
            memberNames: new[] { nameof(Names) }
          ));
        }
      }

      if (Speeds != null)
      {
        IEnumerable<SpeedType> speedTypes = Speeds.GroupBy(x => x.Type)
          .Where(x => x.Count() > 1)
          .Select(x => x.Key);
        if (speedTypes.Any())
        {
          results.Add(new ValidationResult(
            errorMessage: $"Each type must only appear once: {string.Join(", ", speedTypes)}.",
            memberNames: new[] { nameof(speedTypes) }
          ));
        }
      }

      return results;
    }
  }
}
