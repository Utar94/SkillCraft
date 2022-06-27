using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Characters.Payloads
{
  public class CharacterPowerPayload : IValidatableObject
  {
    public Guid? Id { get; set; }

    public Guid? PowerId { get; set; }

    [Range(0, 5)]
    public int Cost { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var results = new List<ValidationResult>(capacity: 1);

      if (Id == null && PowerId == null)
      {
        results.Add(new ValidationResult(
          errorMessage: $"At least one of the following must be provided: {nameof(Id)}, {nameof(PowerId)}."
        ));
      }
      else if (Id.HasValue && PowerId.HasValue)
      {
        results.Add(new ValidationResult(
          errorMessage: $"Only one of the following must be provided: {nameof(Id)}, {nameof(PowerId)}."
        ));
      }

      return results;
    }
  }
}
