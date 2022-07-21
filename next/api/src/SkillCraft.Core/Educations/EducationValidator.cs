using FluentValidation;

namespace SkillCraft.Core.Educations
{
  internal class EducationValidator : AbstractValidator<Education>
  {
    public EducationValidator()
    {
      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);

      RuleFor(x => x.WealthMultiplier)
        .GreaterThanOrEqualTo(0);
    }
  }
}
