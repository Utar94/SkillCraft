using SkillCraft.Core.Languages;
using SkillCraft.Core.Races;

namespace SkillCraft.Infrastructure.Entities
{
  internal class RaceLanguage
  {
    public Race? Race { get; set; }
    public int RaceSid { get; set; }

    public Language? Language { get; set; }
    public int LanguageSid { get; set; }
  }
}
