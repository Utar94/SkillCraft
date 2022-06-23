using Logitar.WebApiToolKit.Core.Exceptions;

namespace SkillCraft.Core.Characters
{
  internal class SpentLearningPointsExceededException : BadRequestException
  {
    public SpentLearningPointsExceededException(Character character)
      : base("SpentLearningPointsExceeded", $"The spent learning points has been exceeded for the character \"{character}\".")
    {
      Character = character ?? throw new ArgumentNullException(nameof(character));
    }

    public Character Character { get; }
  }
}
