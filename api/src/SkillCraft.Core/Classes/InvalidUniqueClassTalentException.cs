using Logitar.WebApiToolKit.Core.Exceptions;
using SkillCraft.Core.Talents;

namespace SkillCraft.Core.Classes
{
  internal class InvalidUniqueClassTalentException : BadRequestException
  {
    public InvalidUniqueClassTalentException(Talent uniqueTalent)
      : base("InvalidUniqueClassTalent", $"The unique talent \"{uniqueTalent}\" cannot be a class talent.")
    {
      UniqueTalent = uniqueTalent ?? throw new ArgumentNullException(nameof(uniqueTalent));
    }

    public Talent UniqueTalent { get; }
  }
}
