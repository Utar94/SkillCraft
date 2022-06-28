using Logitar.WebApiToolKit.Core.Exceptions;
using SkillCraft.Core.Talents;

namespace SkillCraft.Core.Classes
{
  internal class UniqueTalentCannotBeRequiredException : BadRequestException
  {
    public UniqueTalentCannotBeRequiredException(Talent uniqueTalent)
      : base("UniqueTalentCannotBeRequired", $"The unique talent \"{uniqueTalent}\" cannot be required by another talent.")
    {
      UniqueTalent = uniqueTalent ?? throw new ArgumentNullException(nameof(uniqueTalent));
    }

    public Talent UniqueTalent { get; }
  }
}
