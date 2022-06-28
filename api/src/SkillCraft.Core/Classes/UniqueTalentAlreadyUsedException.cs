using Logitar.WebApiToolKit.Core.Exceptions;
using SkillCraft.Core.Talents;

namespace SkillCraft.Core.Classes
{
  internal class UniqueTalentAlreadyUsedException : ConflictException
  {
    public UniqueTalentAlreadyUsedException(Talent uniqueTalent, string paramName)
      : base(paramName, $"The unique talent \"{uniqueTalent}\" is already used by a class.")
    {
      UniqueTalent = uniqueTalent ?? throw new ArgumentNullException(nameof(uniqueTalent));
    }

    public Talent UniqueTalent { get; }
  }
}
