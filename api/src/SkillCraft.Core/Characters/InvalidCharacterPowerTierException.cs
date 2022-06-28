using Logitar.WebApiToolKit.Core.Exceptions;
using SkillCraft.Core.Powers;
using System.Text;

namespace SkillCraft.Core.Characters
{
  internal class InvalidCharacterPowerTierException : BadRequestException
  {
    public InvalidCharacterPowerTierException(int tier, Power power)
      : base("InvalidCharacterPowerTier", GetMessage(tier, power))
    {
      Power = power ?? throw new ArgumentNullException(nameof(power));
      Tier = tier;
    }

    public Power Power { get; }
    public int Tier { get; }

    private static string GetMessage(int tier, Power power)
    {
      var message = new StringBuilder();

      message.AppendLine("The character cannot learn the specified power.");
      message.AppendLine($"Character tier: {tier}");
      message.AppendLine($"Power: {power} (Tier={power?.Tier}");

      return message.ToString();
    }
  }
}
