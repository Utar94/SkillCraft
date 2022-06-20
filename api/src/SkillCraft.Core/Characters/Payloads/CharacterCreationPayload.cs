using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Characters.Payloads
{
  public class CharacterCreationPayload : IValidatableObject
  {
    [Required]
    public AttributeBasesPayload AttributeBases { get; set; } = null!;

    public Attribute BestAttribute { get; set; }
    public Attribute WorstAttribute { get; set; }
    public Attribute MandatoryAttribute1 { get; set; }
    public Attribute MandatoryAttribute2 { get; set; }
    public Attribute OptionalAttribute1 { get; set; }
    public Attribute OptionalAttribute2 { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var results = new List<ValidationResult>(capacity: 1);

      if (BestAttribute == WorstAttribute)
      {
        results.Add(new ValidationResult($"The {nameof(BestAttribute)} must be different from the {nameof(WorstAttribute)}."));
      }

      return results;
    }
  }
}
