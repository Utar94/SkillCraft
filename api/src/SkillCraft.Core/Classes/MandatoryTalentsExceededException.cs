using Logitar.WebApiToolKit.Core.Exceptions;

namespace SkillCraft.Core.Classes
{
  internal class MandatoryTalentsExceededException : BadRequestException
  {
    public MandatoryTalentsExceededException(Class @class)
      : base("MandatoryTalentsExceeded", $"The number of mandatory talents has been exceeded for the class \"{@class}\".")
    {
      Class = @class ?? throw new ArgumentNullException(nameof(@class));
    }

    public Class Class { get; }
  }
}
