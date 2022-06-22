using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Characters.Payloads
{
  public class SaveCharacterStep3Payload : IValidatableObject
  {
    public Guid NatureId { get; set; }

    public IEnumerable<Guid>? CustomizationIds { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var results = new List<ValidationResult>(capacity: 1);

      if (CustomizationIds != null)
      {
        IEnumerable<Guid> customizationIds = CustomizationIds.GroupBy(x => x)
          .Where(x => x.Count() > 1)
          .Select(x => x.Key);
        if (customizationIds.Any())
        {
          results.Add(new ValidationResult(
            errorMessage: $"Each customization must only appear once: {string.Join(", ", customizationIds)}.",
            memberNames: new[] { nameof(CustomizationIds) }
          ));
        }
      }

      return results;
    }
  }
}
