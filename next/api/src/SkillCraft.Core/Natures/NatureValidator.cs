using FluentValidation;
using SkillCraft.Core.Customizations;

namespace SkillCraft.Core.Natures
{
  internal class NatureValidator : AbstractValidator<Nature>
  {
    public NatureValidator()
    {
      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);

      RuleFor(x => x.Feat)
        .Must(feat => feat == null || feat.Type == CustomizationType.Feat);
    }
  }
}
