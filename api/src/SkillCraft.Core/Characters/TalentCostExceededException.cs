using Logitar.WebApiToolKit.Core.Exceptions;
using SkillCraft.Core.Talents;
using System.Text;

namespace SkillCraft.Core.Characters
{
  internal class TalentCostExceededException : BadRequestException
  {
    public TalentCostExceededException(Talent talent, int cost)
      : base("TalentCostExceeded", GetMessage(talent, cost))
    {
      Cost = cost;
      Talent = talent ?? throw new ArgumentNullException(nameof(talent));
    }

    public int Cost { get; }
    public Talent Talent { get; }

    private static string GetMessage(Talent talent, int cost)
    {
      var message = new StringBuilder();

      message.AppendLine("The maximum talent cost has been exceeded.");
      message.AppendLine($"Tier: {talent?.Tier}");
      message.AppendLine($"Cost: {cost}");

      return message.ToString();
    }
  }
}
