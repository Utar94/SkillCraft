using Logitar.Validation;
using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Powers.Payloads
{
  public abstract class SavePowerPayload : IValidatableObject
  {
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    public DescriptionsPayload? Descriptions { get; set; }

    [Enum(typeof(IncantationType))]
    public IncantationType Incantation { get; set; }

    [Range(0, 3600)]
    public int? Duration { get; set; }

    [Range(0, 1056)]
    public int? Range { get; set; }

    [StringLength(100)]
    public string? Ingredients { get; set; }

    public bool Concentration { get; set; }

    public bool Focus { get; set; }

    public bool Ritual { get; set; }

    public bool Somatic { get; set; }

    public bool Verbal { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var results = new List<ValidationResult>(capacity: 2);

      if (Concentration && Duration == 0)
      {
        results.Add(new ValidationResult(
          errorMessage: $"An instantaneous power cannot use {nameof(Concentration)}.",
          memberNames: new[] { nameof(Concentration) }
        ));
      }

      if (Focus && Ingredients == null)
      {
        results.Add(new ValidationResult(
          errorMessage: $"{nameof(Ingredients)} are required in order to have the {nameof(Focus)} component.",
          memberNames: new[] { nameof(Focus) }
        ));
      }

      return results;
    }
  }
}
