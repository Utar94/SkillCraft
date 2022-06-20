using SkillCraft.Core.Languages.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Races.Models
{
  public class RaceModel : EntityBaseModel
  {
    public RaceModel? Parent { get; set; }

    public string? Description { get; set; }
    public string Name { get; set; } = null!;

    public IEnumerable<AttributeBonusModel> Attributes { get; set; } = null!;
    public IEnumerable<NameCategoryModel> Names { get; set; } = null!;
    public IEnumerable<RacialSpeedModel> Speeds { get; set; } = null!;

    public AgeThresholdsModel? AgeThresholds { get; set; }
    public SizeCategory Size { get; set; }
    public string? StatureRoll { get; set; }
    public WeightRollsModel? WeightRolls { get; set; }

    public IEnumerable<LanguageModel> Languages { get; set; } = null!;
    public IEnumerable<RacialTraitModel> Traits { get; set; } = null!;

    public int ExtraAttributes { get; set; }
    public int ExtraLanguages { get; set; }

    public string? AgeText { get; set; }
    public string? AttributesText { get; set; }
    public string? LanguagesText { get; set; }
    public string? NamesText { get; set; }
    public string? PeopleText { get; set; }
    public string? SizeText { get; set; }
    public string? SpeedText { get; set; }
    public string? TraitsText { get; set; }
    public string? WeightText { get; set; }
  }
}
