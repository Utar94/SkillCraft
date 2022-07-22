using FluentValidation;

namespace SkillCraft.Core.Races
{
  internal class RaceValidator : AbstractValidator<Race>
  {
    public RaceValidator()
    {
      RuleFor(x => x.Parent)
        .Must(x => x?.ParentSid == null);

      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);

      RuleForEach(x => x.Attributes.Values)
        .InclusiveBetween(0, 9);

      RuleFor(x => x.ExtraAttributes)
        .InclusiveBetween(0, 3);

      RuleFor(x => x.AttributesText)
        .MaximumLength(1000);

      RuleFor(x => x.NamesText)
        .MaximumLength(1000);

      RuleForEach(x => x.Speeds.Values)
        .InclusiveBetween(0, 99);

      RuleFor(x => x.SpeedsText)
        .MaximumLength(1000);

      RuleFor(x => x.AgeThresholds)
        .Must(x => x == null || (x.Length == 4 && x.SequenceEqual(x.OrderBy(y => y))));

      RuleFor(x => x.StatureRoll)
        .Matches(Constants.RollPattern);

      RuleFor(x => x.WeightRolls)
        .Must(x => x == null || x.Length == 5)
        .ForEach(x => x.Matches(Constants.RollPattern));

      RuleFor(x => x.AgeText)
        .MaximumLength(1000);

      RuleFor(x => x.SizeText)
        .MaximumLength(1000);

      RuleFor(x => x.WeightText)
        .MaximumLength(1000);

      RuleFor(x => x.ExtraLanguages)
        .InclusiveBetween(0, 3);

      RuleFor(x => x.LanguagesText)
        .MaximumLength(1000);

      RuleForEach(x => x.Traits)
        .SetValidator(x => new RacialTraitValidator(x));

      RuleFor(x => x.TraitsText)
        .MaximumLength(1000);

      RuleFor(x => x)
        .Must(x => !x.People.Any(y => y.ParentSid != x.Sid || y.People.Any()));

      RuleFor(x => x.PeopleText)
        .MaximumLength(1000);
    }
  }
}
