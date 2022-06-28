using Logitar.WebApiToolKit.Core.Exceptions;
using SkillCraft.Core.Talents;
using System.Text;

namespace SkillCraft.Core.Characters
{
  internal class MissingRequiredTalentsException : BadRequestException
  {
    public MissingRequiredTalentsException(Character character, IEnumerable<Talent> requiredTalents)
      : base("MissingRequiredTalents", GetMessage(character, requiredTalents))
    {
      Character = character ?? throw new ArgumentNullException(nameof(character));
      RequiredTalents = requiredTalents ?? throw new ArgumentNullException(nameof(requiredTalents));
    }

    public Character Character { get; }
    public IEnumerable<Talent> RequiredTalents { get; }

    private static string GetMessage(Character character, IEnumerable<Talent> requiredTalents)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified character hasn't learn the required talents.");
      message.AppendLine($"Character: {character}");
      message.AppendLine($"Required talents: {requiredTalents?.Select(x => x.Id).Distinct()}");

      return message.ToString();
    }
  }
}
