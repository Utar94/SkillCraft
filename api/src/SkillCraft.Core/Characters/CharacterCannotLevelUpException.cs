using Logitar.WebApiToolKit.Core.Exceptions;

namespace SkillCraft.Core.Characters
{
  internal class CharacterCannotLevelUpException : BadRequestException
  {
    public CharacterCannotLevelUpException(Character character)
      : base("CharacterCannotLevelUp", $"The character \"{character}\" cannot be leveled-up.")
    {
      Character = character ?? throw new ArgumentNullException(nameof(character));
    }

    public Character Character { get; }
  }
}
