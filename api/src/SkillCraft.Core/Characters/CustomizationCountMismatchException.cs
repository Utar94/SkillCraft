using Logitar.WebApiToolKit.Core.Exceptions;

namespace SkillCraft.Core.Characters
{
  public class CustomizationCountMismatchException : BadRequestException
  {
    public CustomizationCountMismatchException(int feats, int disabilities)
      : base("CustomizationCountMismatch", $"The number of feats ({feats}) does not match the number of disabilities ({disabilities}).")
    {
      Disabilities = disabilities;
      Feats = feats;
    }

    public int Disabilities { get; }
    public int Feats { get; }
  }
}
