using Logitar.WebApiToolKit.Core.Exceptions;

namespace SkillCraft.Core.Characters
{
  internal class NatureFeatUnexpectedException : BadRequestException
  {
    public NatureFeatUnexpectedException()
      : base("NatureFeatUnexpected", "The nature feat was unexpected.")
    {
    }
  }
}
