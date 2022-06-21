using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Characters.Payloads
{
  public class CreateBonusPayload : IValidatableObject
  {
    public BonusType Type { get; set; }

    [Required]
    public string Target { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    public bool Permanent { get; set; }

    public int Value { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var results = new List<ValidationResult>(capacity: 2);

      switch (Type)
      {
        case BonusType.Attribute:
          if (!Enum.TryParse<Attribute>(Target, out _))
          {
            results.Add(new ValidationResult("The value is not a valid attribute.", new[] { nameof(Target) }));
          }
          break;
        case BonusType.Other:
          if (!Enum.TryParse<OtherBonusTarget>(Target, out _))
          {
            results.Add(new ValidationResult("The value is not a valid other #bonus target.", new[] { nameof(Target) }));
          }
          break;
        case BonusType.Skill:
          if (!Enum.TryParse<Skill>(Target, out _))
          {
            results.Add(new ValidationResult("The value is not a valid skill.", new[] { nameof(Target) }));
          }
          break;
        case BonusType.Statistic:
          if (!Enum.TryParse<Statistic>(Target, out _))
          {
            results.Add(new ValidationResult("The value is not a valid statistic.", new[] { nameof(Target) }));
          }
          break;
        default:
          results.Add(new ValidationResult("The value is not a valid bonus type.", new[] { nameof(Type) }));
          break;
      }

      if (Value == 0)
      {
        results.Add(new ValidationResult("The value must differ from 0.", new[] { nameof(Value) }));
      }

      return results;
    }
  }
}
