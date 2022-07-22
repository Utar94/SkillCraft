using FluentValidation;

namespace SkillCraft.Core.Races
{
  internal class RacialTraitValidator : AbstractValidator<RacialTrait>
  {
    public RacialTraitValidator(Race? race = null)
    {
      if (race != null)
      {
        RuleFor(x => x.Race)
          .Must(x => x?.Equals(race) != false);

        RuleFor(x => x.RaceSid)
          .Must(x => x == race.Sid);
      }

      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);
    }
  }
}
