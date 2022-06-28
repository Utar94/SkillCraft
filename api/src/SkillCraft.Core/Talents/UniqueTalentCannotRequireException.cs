using Logitar.WebApiToolKit.Core.Exceptions;
using System.Text;

namespace SkillCraft.Core.Talents
{
  internal class UniqueTalentCannotRequireException : BadRequestException
  {
    public UniqueTalentCannotRequireException(Talent talent)
      : base("UniqueTalentCannotRequire", GetMessage(talent))
    {
      Talent = talent ?? throw new ArgumentNullException(nameof(talent));
    }

    public Talent Talent { get; }

    private static string GetMessage(Talent talent)
    {
      var message = new StringBuilder();

      message.AppendLine("An unique talent cannot require another talent.");
      message.AppendLine($"Talent: {talent}");

      return message.ToString();
    }
  }
}
