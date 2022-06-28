using Logitar.WebApiToolKit.Core.Exceptions;
using System.Text;

namespace SkillCraft.Core.Talents
{
  internal class CannotRequireUniqueTalentException : BadRequestException
  {
    public CannotRequireUniqueTalentException(Talent requiredTalent, Talent requiringTalent)
      : base("CannotRequireUniqueTalent", GetMessage(requiredTalent, requiringTalent))
    {
      RequiredTalent = requiredTalent ?? throw new ArgumentNullException(nameof(requiredTalent));
      RequiringTalent = requiringTalent ?? throw new ArgumentNullException(nameof(requiringTalent));
    }

    public Talent RequiredTalent { get; }
    public Talent RequiringTalent { get; }

    private static string GetMessage(Talent requiredTalent, Talent requiringTalent)
    {
      var message = new StringBuilder();

      message.AppendLine("A talent cannot require an unique talent.");
      message.AppendLine($"Required talent: {requiredTalent}");
      message.AppendLine($"Requiring talent: {requiringTalent}");

      return message.ToString();
    }
  }
}
