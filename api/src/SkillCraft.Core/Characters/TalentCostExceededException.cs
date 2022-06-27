using Logitar.WebApiToolKit.Core.Exceptions;
using SkillCraft.Core.Powers;
using SkillCraft.Core.Talents;
using System.Text;

namespace SkillCraft.Core.Characters
{
  internal class TalentCostExceededException : BadRequestException
  {
    public TalentCostExceededException(Power power, int cost)
      : base("TalentCostExceeded", GetMessage(power?.Tier, cost))
    {
      Cost = cost;
      Power = power ?? throw new ArgumentNullException(nameof(power));
    }
    public TalentCostExceededException(Talent talent, int cost)
      : base("TalentCostExceeded", GetMessage(talent?.Tier, cost))
    {
      Cost = cost;
      Talent = talent ?? throw new ArgumentNullException(nameof(talent));
    }

    public int Cost { get; }
    public Power? Power { get; }
    public Talent? Talent { get; }

    private static string GetMessage(int? tier, int cost)
    {
      var message = new StringBuilder();

      message.AppendLine("The maximum talent cost has been exceeded.");
      message.AppendLine($"Tier: {tier}");
      message.AppendLine($"Cost: {cost}");

      return message.ToString();
    }
  }
}
