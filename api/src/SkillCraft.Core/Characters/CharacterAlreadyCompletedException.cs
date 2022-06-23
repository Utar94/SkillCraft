using Logitar.WebApiToolKit.Core.Exceptions;

namespace SkillCraft.Core.Characters
{
  internal class CharacterAlreadyCompletedException : BadRequestException
  {
    public CharacterAlreadyCompletedException(Character character)
      : base("CharacterAlreadyCompleted", $"The character \"{character}\" has already been completed.")
    {
      Character = character ?? throw new ArgumentNullException(nameof(character));
    }

    public Character Character { get; }
  }
}
