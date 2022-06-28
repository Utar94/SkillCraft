using Logitar.WebApiToolKit.Core.Exceptions;
using SkillCraft.Core.Talents;
using System.Text;

namespace SkillCraft.Core.Characters
{
  internal class InvalidCharacterTalentTierException : BadRequestException
  {
    public InvalidCharacterTalentTierException(int tier, Talent talent)
      : base("InvalidCharacterTalentTier", GetMessage(tier, talent))
    {
      Talent = talent ?? throw new ArgumentNullException(nameof(talent));
      Tier = tier;
    }

    public Talent Talent { get; }
    public int Tier { get; }

    private static string GetMessage(int tier, Talent talent)
    {
      var message = new StringBuilder();

      message.AppendLine("The character cannot learn the specified talent.");
      message.AppendLine($"Character tier: {tier}");
      message.AppendLine($"Talent: {talent} (Tier={talent?.Tier}");

      return message.ToString();
    }
  }
}
