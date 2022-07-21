using FluentValidation;

namespace SkillCraft.Core.Aspects
{
  internal class AspectValidator : AbstractValidator<Aspect>
  {
    public AspectValidator()
    {
      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);

      RuleFor(x => x.MandatoryAttribute1)
        .NotEqual(x => x.MandatoryAttribute2)
        .NotEqual(x => x.OptionalAttribute1)
        .NotEqual(x => x.OptionalAttribute2);
      RuleFor(x => x.MandatoryAttribute2)
        .NotEqual(x => x.OptionalAttribute1)
        .NotEqual(x => x.OptionalAttribute2);
      RuleFor(x => x.OptionalAttribute1)
        .NotEqual(x => x.OptionalAttribute2);

      RuleFor(x => x.Skill1)
        .NotEqual(x => x.Skill2);
    }
  }
}
