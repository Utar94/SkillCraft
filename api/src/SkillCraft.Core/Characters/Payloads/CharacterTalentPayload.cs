using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Characters.Payloads
{
  public class CharacterTalentPayload : IValidatableObject
  {
    public Guid? Id { get; set; }

    public Guid? TalentId { get; set; }

    public Guid? OptionId { get; set; }

    [Range(0, 5)]
    public int Cost { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var results = new List<ValidationResult>(capacity: 1);

      if (Id == null && TalentId == null)
      {
        results.Add(new ValidationResult(
          errorMessage: $"At least one of the following must be provided: {nameof(Id)}, {nameof(TalentId)}."
        ));
      }
      else if (Id.HasValue && TalentId.HasValue)
      {
        results.Add(new ValidationResult(
          errorMessage: $"Only one of the following must be provided: {nameof(Id)}, {nameof(TalentId)}."
        ));
      }

      return results;
    }
  }
}
