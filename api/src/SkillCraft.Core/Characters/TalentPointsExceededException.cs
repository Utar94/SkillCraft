using Logitar.WebApiToolKit.Core.Exceptions;

namespace SkillCraft.Core.Characters
{
  internal class SpentTalentPointsExceededException : BadRequestException
  {
    public SpentTalentPointsExceededException(Character character)
      : base("SpentTalentPointsExceeded", $"The spent talent points has been exceeded for the character \"{character}\".")
    {
      Character = character ?? throw new ArgumentNullException(nameof(character));
    }

    public Character Character { get; }
  }
}
