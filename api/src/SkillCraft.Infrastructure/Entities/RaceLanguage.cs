using SkillCraft.Core.Languages;
using SkillCraft.Core.Races;

namespace SkillCraft.Infrastructure.Entities
{
  internal class RaceLanguage
  {
    public int RaceId { get; set; }
    public Race? Race { get; set; }

    public int LanguageId { get; set; }
    public Language? Language { get; set; }
  }
}
