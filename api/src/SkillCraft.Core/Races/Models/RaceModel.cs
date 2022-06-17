using SkillCraft.Core.Languages.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Races.Models
{
  public class RaceModel : EntityBaseModel
  {
    public string? Description { get; set; }
    public string Name { get; set; } = null!;

    public IEnumerable<AttributeBonusModel> Attributes { get; set; } = null!;
    public IEnumerable<NameCategoryModel> Names { get; set; } = null!;
    public IEnumerable<RacialSpeedModel> Speeds { get; set; } = null!;

    public IEnumerable<int> AgeThresholds { get; set; } = null!;
    public SizeCategory Size { get; set; }
    public string? StatureRoll { get; set; }
    public IEnumerable<string> WeightRolls { get; set; } = null!;

    public IEnumerable<LanguageModel> Languages { get; set; } = null!;
    public IEnumerable<RacialTraitModel> Traits { get; set; } = null!;

    public int ExtraAttributes { get; set; }
    public int ExtraLanguages { get; set; }

    public string? AgeText { get; set; }
    public string? AttributesText { get; set; }
    public string? LanguagesText { get; set; }
    public string? NamesText { get; set; }
    public string? SizeText { get; set; }
    public string? SpeedText { get; set; }
    public string? SubraceText { get; set; }
    public string? TraitsText { get; set; }
    public string? WeightText { get; set; }
  }
}
