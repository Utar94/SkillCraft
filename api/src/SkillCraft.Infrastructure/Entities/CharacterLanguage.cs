using SkillCraft.Core.Characters;
using SkillCraft.Core.Languages;

namespace SkillCraft.Infrastructure.Entities
{
  internal class CharacterLanguage
  {
    public int CharacterId { get; set; }
    public Character? Character { get; set; }

    public int LanguageId { get; set; }
    public Language? Language { get; set; }
  }
}
