using Logitar.Validation;
using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Aspects.Payloads
{
  public abstract class SaveAspectPayload : IValidatableObject
  {
    public string? Description { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    #region Attributes
    [Enum(typeof(Attribute))]
    public Attribute? MandatoryAttribute1 { get; set; }

    [Enum(typeof(Attribute))]
    public Attribute? MandatoryAttribute2 { get; set; }

    [Enum(typeof(Attribute))]
    public Attribute? OptionalAttribute1 { get; set; }

    [Enum(typeof(Attribute))]
    public Attribute? OptionalAttribute2 { get; set; }
    #endregion

    #region Skills
    [Enum(typeof(Skill))]
    public Skill? Skill1 { get; set; }

    [Enum(typeof(Skill))]
    public Skill? Skill2 { get; set; }
    #endregion

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var results = new List<ValidationResult>(capacity: 2);

      IEnumerable<Attribute> attributes = new[]
      {
        MandatoryAttribute1,
        MandatoryAttribute2,
        OptionalAttribute1,
        OptionalAttribute2
      }
        .Where(x => x.HasValue)
        .Select(x => x!.Value)
        .GroupBy(x => x)
        .Where(x => x.Count() > 1)
        .Select(x => x.Key)
        .OrderBy(x => x);

      if (attributes.Any())
      {
        results.Add(new ValidationResult(
          errorMessage: $"Each attribute can only be used once. The duplicates are: {string.Join(", ", attributes)}.",
          memberNames: Array.Empty<string>()
        ));
      }

      if (Skill1.HasValue && Skill2.HasValue && Skill1.Value == Skill2.Value)
      {
        results.Add(new ValidationResult(
          errorMessage: "The skills must be different.",
          memberNames: Array.Empty<string>()
        ));
      }

      return results;
    }
  }
}
