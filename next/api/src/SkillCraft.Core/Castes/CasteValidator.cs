using FluentValidation;

namespace SkillCraft.Core.Castes
{
  internal class CasteValidator : AbstractValidator<Caste>
  {
    public CasteValidator()
    {
      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);

      RuleFor(x => x.WealthRoll)
        .Matches(Constants.RollPattern);
    }
  }
}
