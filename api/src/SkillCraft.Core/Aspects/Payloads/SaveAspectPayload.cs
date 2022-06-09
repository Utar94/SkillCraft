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

    [Enum(typeof(Attribute))]
    public Attribute MandatoryAttribute1 { get; set; }

    [Enum(typeof(Attribute))]
    public Attribute MandatoryAttribute2 { get; set; }

    [Enum(typeof(Attribute))]
    public Attribute OptionalAttribute1 { get; set; }

    [Enum(typeof(Attribute))]
    public Attribute OptionalAttribute2 { get; set; }

    [Enum(typeof(Skill))]
    public Skill Skill1 { get; set; }

    [Enum(typeof(Skill))]
    public Skill Skill2 { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var results = new List<ValidationResult>(capacity: 2);

      Dictionary<Attribute, int> attributes = new[] { MandatoryAttribute1, MandatoryAttribute2, OptionalAttribute1, OptionalAttribute2 }
        .GroupBy(x => x)
        .Where(x => x.Count() > 1)
        .ToDictionary(x => x.Key, x => x.Count());
      if (attributes.Any())
      {
        results.Add(new ValidationResult($"An attribute should only appear once. Conflicts: {string.Join(", ", attributes.Keys)}"));
      }

      if (Skill1 == Skill2)
      {
        results.Add(new ValidationResult("The two Skills should differ."));
      }

      return results;
    }
  }
}
