using Logitar.Validation;
using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Characters.Payloads
{
  public class SkillRankPayload : IValidatableObject
  {
    public Guid? Id { get; set; }

    [Enum(typeof(Skill))]
    public Skill? Skill { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var results = new List<ValidationResult>(capacity: 1);

      if (Id == null && Skill == null)
      {
        results.Add(new ValidationResult(
          errorMessage: $"At least one of the following must be provided: {nameof(Id)}, {nameof(Skill)}."
        ));
      }
      else if (Id.HasValue && Skill.HasValue)
      {
        results.Add(new ValidationResult(
          errorMessage: $"Only one of the following must be provided: {nameof(Id)}, {nameof(Skill)}."
        ));
      }

      return results;
    }
  }
}
