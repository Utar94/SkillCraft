using FluentValidation;

namespace SkillCraft.Core.Customizations
{
  internal class CustomizationValidator : AbstractValidator<Customization>
  {
    public CustomizationValidator()
    {
      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);
    }
  }
}
