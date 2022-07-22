using FluentValidation;

namespace SkillCraft.Core.Talents
{
  internal class TalentValidator : AbstractValidator<Talent>
  {
    public TalentValidator(Talent? requiringTalent = null)
    {
      When(x => x.RequiredTalent != null,
        () => RuleFor(x => x.RequiredTalent!).SetValidator(x => new TalentValidator(x)));

      RuleFor(x => x.Tier)
        .InclusiveBetween(0, requiringTalent?.Tier ?? 3);

      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);

      RuleForEach(x => x.Options)
        .SetValidator(x => new TalentOptionValidator(x));

      // TODO(fpion): CannotRequireUniqueTalentException
      // TODO(fpion): UniqueTalentCannotRequireException
    }
  }
}
