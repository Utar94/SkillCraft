using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Characters.Payloads
{
  public class SaveCharacterStep2Payload : IValidatableObject
  {
    public Guid Aspect1Id { get; set; }

    public Guid Aspect2Id { get; set; }

    [Required]
    public CharacterCreationPayload Creation { get; set; } = null!;

    public Guid RaceId { get; set; }

    [Range(0, 99.99)]
    public double Stature { get; set; }

    [Range(0, 999.9)]
    public double Weight { get; set; }

    [Range(0, 9999)]
    public int Age { get; set; }

    public IEnumerable<Guid>? LanguageIds { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    public IEnumerable<BonusPayload>? Bonuses { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var results = new List<ValidationResult>(capacity: 3);

      if (Aspect1Id == Aspect2Id)
      {
        results.Add(new ValidationResult(
          errorMessage: $"The {nameof(Aspect1Id)} must differ from the {nameof(Aspect2Id)}."
        ));
      }

      if (Bonuses != null)
      {
        IEnumerable<Guid> bonusIds = Bonuses.GroupBy(x => x.Id)
          .Where(x => x.Key.HasValue && x.Count() > 1)
          .Select(x => x.Key!.Value);
        if (bonusIds.Any())
        {
          results.Add(new ValidationResult(
            errorMessage: $"Each bonus must only appear once: {string.Join(", ", bonusIds)}.",
            memberNames: new[] { nameof(Bonuses) }
          ));
        }
      }

      if (LanguageIds != null)
      {
        IEnumerable<Guid> languageIds = LanguageIds.GroupBy(x => x)
          .Where(x => x.Count() > 1)
          .Select(x => x.Key);
        if (languageIds.Any())
        {
          results.Add(new ValidationResult(
            errorMessage: $"Each language must only appear once: {string.Join(", ", languageIds)}.",
            memberNames: new[] { nameof(LanguageIds) }
          ));
        }
      }

      return results;
    }
  }
}
