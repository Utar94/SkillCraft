using FluentValidation;

namespace SkillCraft.Core.Languages
{
  internal class LanguageValidator : AbstractValidator<Language>
  {
    public LanguageValidator()
    {
      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);

      RuleFor(x => x.Script)
        .MaximumLength(100);

      RuleFor(x => x.TypicalSpeakers)
       .MaximumLength(100);
    }
  }
}
