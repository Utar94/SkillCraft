using Logitar.WebApiToolKit.Core.Exceptions;
using System.Text;

namespace SkillCraft.Core.Talents
{
  internal class InvalidRequiredTalentTierException : BadRequestException
  {
    public InvalidRequiredTalentTierException(Talent requiredTalent, Talent requiringTalent)
      : base("InvalidRequiredTalentTier", GetMessage(requiredTalent, requiringTalent))
    {
      RequiredTalent = requiredTalent ?? throw new ArgumentNullException(nameof(requiredTalent));
      RequiringTalent = requiringTalent ?? throw new ArgumentNullException(nameof(requiringTalent));
    }

    public Talent RequiredTalent { get; }
    public Talent RequiringTalent { get; }

    private static string GetMessage(Talent requiredTalent, Talent requiringTalent)
    {
      var message = new StringBuilder();

      message.AppendLine("The required talent tier must be lower or equal to the requiring talent tier.");
      message.AppendLine($"Required talent: {requiredTalent} (Tier={requiredTalent?.Tier}");
      message.AppendLine($"Requiring talent: {requiringTalent} (Tier={requiringTalent?.Tier}");

      return message.ToString();
    }
  }
}
