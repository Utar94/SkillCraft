using Logitar.WebApiToolKit.Core.Exceptions;
using System.Text;

namespace SkillCraft.Core.Conditions
{
  internal class ConditionLevelExceededException : BadRequestException
  {
    public ConditionLevelExceededException(Condition condition, int level)
      : base("ConditionLevelExceeded", GetMessage(condition, level))
    {
      Condition = condition ?? throw new ArgumentNullException(nameof(condition));
      Level = level;
    }

    public Condition Condition { get; }
    public int Level { get; }

    private static string GetMessage(Condition condition, int level)
    {
      var message = new StringBuilder();

      message.AppendLine("The condition level has been exceeded.");
      message.AppendLine($"Condition: {condition} (MaxLevel: {condition?.MaxLevel})");
      message.AppendLine($"Level: {level}");

      return message.ToString();
    }
  }
}
