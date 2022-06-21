using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Characters.Payloads
{
  public class BonusPayload : IValidatableObject
  {
    public Guid? Id { get; set; }

    public BonusType? Type { get; set; }

    public string? Target { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    public bool Permanent { get; set; }

    public int Value { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var results = new List<ValidationResult>(capacity: 3);

      if (Id.HasValue)
      {
        if (Type.HasValue)
        {
          results.Add(new ValidationResult($"The value must be null when an {nameof(Id)} is provided.", new[] { nameof(Type) }));
        }
        if (Target != null)
        {
          results.Add(new ValidationResult($"The value must be null when an {nameof(Id)} is provided.", new[] { nameof(Target) }));
        }
      }
      else if (Type.HasValue)
      {
        switch (Type.Value)
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
      }
      else
      {
        results.Add(new ValidationResult($"The value is required when no {nameof(Id)} is provided.", new[] { nameof(Type) }));

        if (Target == null)
        {
          results.Add(new ValidationResult($"The value is required when no {nameof(Id)} is provided.", new[] { nameof(Target) }));
        }
      }

      if (Value == 0)
      {
        results.Add(new ValidationResult("The value must differ from 0.", new[] { nameof(Value) }));
      }

      return results;
    }
  }
}
