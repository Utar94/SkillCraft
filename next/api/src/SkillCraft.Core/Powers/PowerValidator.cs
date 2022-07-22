using FluentValidation;

namespace SkillCraft.Core.Powers
{
  internal class PowerValidator : AbstractValidator<Power>
  {
    public PowerValidator()
    {
      RuleFor(x => x.Tier)
        .InclusiveBetween(0, 3);

      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleForEach(x => x.Descriptions)
        .MaximumLength(1000);

      RuleFor(x => x.Duration)
        .GreaterThanOrEqualTo(0);

      When(x => x.Duration == 0,
        () => RuleFor(x => x.IsConcentration).Equal(false));

      RuleFor(x => x.Range)
        .GreaterThanOrEqualTo(0);

      RuleFor(x => x.Ingredients)
        .MaximumLength(1000);

      When(x => string.IsNullOrWhiteSpace(x.Ingredients),
        () => RuleFor(x => x.IsFocus).Equal(false));
    }
  }
}
