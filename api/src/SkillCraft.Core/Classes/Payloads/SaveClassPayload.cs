using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Classes.Payloads
{
  public abstract class SaveClassPayload : IValidatableObject
  {
    public Guid UniqueTalentId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [StringLength(1000)]
    public string? OtherRequirements { get; set; }

    [StringLength(100)]
    public string? OtherTalentsText { get; set; }

    public IEnumerable<ClassTalentPayload>? Talents { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var results = new List<ValidationResult>(capacity: 2);

      if (Talents != null)
      {
        IEnumerable<Guid> talentIds = Talents.GroupBy(x => x.TalentId)
          .Where(x => x.Count() > 1)
          .Select(x => x.Key);
        if (talentIds.Any())
        {
          results.Add(new ValidationResult(
            errorMessage: $"Each talent must only appear once: {string.Join(", ", talentIds)}.",
            memberNames: new[] { nameof(Talents) }
          ));
        }

        if (Talents.Any(talent => talent.TalentId == UniqueTalentId))
        {
          results.Add(new ValidationResult(
            errorMessage: "The unique talent cannot be included.",
            memberNames: new[] { nameof(Talents) }
          ));
        }
      }

      return results;
    }
  }
}
