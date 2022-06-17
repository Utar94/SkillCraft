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

    public IEnumerable<int>? AgeThresholds { get; set; }

    public SizeCategory Size { get; set; }

    [RegularExpression("\\d{1,2}d\\d{1,2}(\\+\\d{1,3})?")] // TODO(fpion): refactor
    public string? StatureRoll { get; set; }

    public IEnumerable<string>? WeightRolls { get; set; }

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
    public string? SizeText { get; set; }

    [StringLength(1000)]
    public string? SpeedText { get; set; }

    [StringLength(1000)]
    public string? SubraceText { get; set; }

    [StringLength(1000)]
    public string? TraitsText { get; set; }

    [StringLength(1000)]
    public string? WeightText { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var results = new List<ValidationResult>(capacity: 5);

      if (AgeThresholds != null && AgeThresholds.ToHashSet().Count != 4)
      {
        results.Add(new ValidationResult(
          "The collection must contain exactly 4 unique items.",
          new[] { nameof(AgeThresholds) }
        ));
      }

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

      if (WeightRolls != null)
      {
        if (WeightRolls.Count() != 5)
        {
          results.Add(new ValidationResult(
            "The collection must contain exactly 5 items.",
            new[] { nameof(WeightRolls) }
          ));
        }

        // [RegularExpression("\\d{1,2}d\\d{1,2}(\\+\\d{1,3})?")] // TODO(fpion): refactor
      }

      return results;
    }
  }
}
