using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Characters.Payloads
{
  public class UpdateCharacterPayload : IValidatableObject
  {
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string? Player { get; set; }

    [Range(0, 99.99)]
    public double Stature { get; set; }

    [Range(0, 999.9)]
    public double Weight { get; set; }

    [Range(0, 9999)]
    public int Age { get; set; }

    [Range(0, 999999)]
    public int Experience { get; set; }

    [Range(0, 999)]
    public int Vitality { get; set; }

    [Range(0, 999)]
    public int Stamina { get; set; }

    [Range(0, 99)]
    public int BloodAlcoholContent { get; set; }

    [Range(0, 99)]
    public int Intoxication { get; set; }

    public IEnumerable<BonusPayload>? Bonuses { get; set; }

    public IEnumerable<CharacterConditionPayload>? Conditions { get; set; }

    public IEnumerable<Guid>? LanguageIds { get; set; }

    public IEnumerable<CharacterPowerPayload>? Powers { get; set; }

    public IEnumerable<CharacterTalentPayload>? Talents { get; set; }

    public IEnumerable<SkillRankPayload>? SkillRanks { get; set; }

    public string? Description { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var results = new List<ValidationResult>(capacity: 6);

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

      if (Conditions != null)
      {
        IEnumerable<Guid> conditionIds = Conditions.GroupBy(x => x.ConditionId)
          .Where(x => x.Count() > 1)
          .Select(x => x.Key);
        if (conditionIds.Any())
        {
          results.Add(new ValidationResult(
            errorMessage: $"Each condition must only appear once: {string.Join(", ", conditionIds)}.",
            memberNames: new[] { nameof(Conditions) }
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

      if (Powers != null)
      {
        IEnumerable<Guid> powerIds = Powers.GroupBy(x => x.Id)
          .Where(x => x.Key.HasValue && x.Count() > 1)
          .Select(x => x.Key!.Value);
        if (powerIds.Any())
        {
          results.Add(new ValidationResult(
            errorMessage: $"Each power must only appear once: {string.Join(", ", powerIds)}.",
            memberNames: new[] { nameof(Powers) }
          ));
        }
      }

      if (SkillRanks != null)
      {
        IEnumerable<Guid> skillRankIds = SkillRanks.GroupBy(x => x.Id)
          .Where(x => x.Key.HasValue && x.Count() > 1)
          .Select(x => x.Key!.Value);
        if (skillRankIds.Any())
        {
          results.Add(new ValidationResult(
            errorMessage: $"Each skill rank must only appear once: {string.Join(", ", skillRankIds)}.",
            memberNames: new[] { nameof(SkillRanks) }
          ));
        }
      }

      if (Talents != null)
      {
        IEnumerable<Guid> talentIds = Talents.GroupBy(x => x.Id)
          .Where(x => x.Key.HasValue && x.Count() > 1)
          .Select(x => x.Key!.Value);
        if (talentIds.Any())
        {
          results.Add(new ValidationResult(
            errorMessage: $"Each talent must only appear once: {string.Join(", ", talentIds)}.",
            memberNames: new[] { nameof(Talents) }
          ));
        }
      }

      return results;
    }
  }
}
