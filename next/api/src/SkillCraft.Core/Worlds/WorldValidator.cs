using FluentValidation;

namespace SkillCraft.Core.Worlds
{
  internal class WorldValidator : AbstractValidator<World>
  {
    public WorldValidator()
    {
      RuleFor(x => x.Alias)
        .NotEmpty()
        .MaximumLength(100)
        .Must(BeAlias);

      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);
    }

    private static bool BeAlias(string value) => value != null && value.Split()
      .All(word => !string.IsNullOrEmpty(word) && word.All(c => char.IsLetterOrDigit(c)));
  }
}
