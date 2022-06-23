using Logitar.WebApiToolKit.Core.Exceptions;

namespace SkillCraft.Core.Talents
{
  internal class TalentOptionRequiredException : BadRequestException
  {
    public TalentOptionRequiredException()
      : base("TalentOptionRequired", "The talent option is required.")
    {
    }
  }
}
