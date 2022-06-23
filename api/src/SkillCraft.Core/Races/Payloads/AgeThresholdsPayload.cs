using Logitar.Validation;
using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Races.Payloads
{
  public class AgeThresholdsPayload : IValidatableObject
  {
    [MinValue(1)]
    public int Teenager { get; set; }

    [MinValue(1)]
    public int Adult { get; set; }

    [MinValue(1)]
    public int Mature { get; set; }

    [MinValue(1)]
    public int Venerable { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var results = new List<ValidationResult>(capacity: 3);

      if (Adult <= Teenager)
      {
        results.Add(new ValidationResult(
          errorMessage: $"The value must be greater than the value of ${nameof(Teenager)}.",
          memberNames: new[] { nameof(Adult) })
        );
      }
      if (Mature <= Adult)
      {
        results.Add(new ValidationResult(
          errorMessage: $"The value must be greater than the value of ${nameof(Adult)}.",
          memberNames: new[] { nameof(Mature) })
        );
      }
      if (Venerable <= Mature)
      {
        results.Add(new ValidationResult(
          errorMessage: $"The value must be greater than the value of ${nameof(Mature)}.",
          memberNames: new[] { nameof(Venerable) })
        );
      }

      return results;
    }
  }
}
