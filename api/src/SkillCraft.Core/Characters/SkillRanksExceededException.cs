using Logitar.WebApiToolKit.Core.Exceptions;
using System.Text;

namespace SkillCraft.Core.Characters
{
  internal class SkillRanksExceededException : BadRequestException
  {
    public SkillRanksExceededException(Character character, IEnumerable<Skill> skills)
      : base("SkillRanksExceeded", GetMessage(character, skills))
    {
      Character = character ?? throw new ArgumentNullException(nameof(character));
      Skills = skills ?? throw new ArgumentNullException(nameof(skills));
    }

    public Character Character { get; }
    public IEnumerable<Skill> Skills { get; }

    private static string GetMessage(Character character, IEnumerable<Skill> skills)
    {
      var message = new StringBuilder();

      message.AppendLine("The skill ranks have been exceeded for the specified character.");
      message.AppendLine($"Character: {character} (Tier={character?.Tier}");
      message.AppendLine($"Skills: {string.Join(", ", skills)}");

      return message.ToString();
    }
  }
}
