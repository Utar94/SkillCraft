using FluentValidation;

namespace SkillCraft.Core.Talents
{
  internal class TalentOptionValidator : AbstractValidator<TalentOption>
  {
    public TalentOptionValidator(Talent? talent = null)
    {
      if (talent != null)
      {
        RuleFor(x => x.Talent)
          .Must(x => x?.Equals(talent) != false);

        RuleFor(x => x.TalentSid)
          .Must(x => x == talent.Sid);
      }

      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);
    }
  }
}
