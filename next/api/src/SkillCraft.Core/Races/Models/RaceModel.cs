using SkillCraft.Core.Languages.Models;

namespace SkillCraft.Core.Races.Models
{
  public class RaceModel : AggregateModel
  {
    public RaceModel? Parent { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public AttributeBonusesModel Attributes { get; set; } = null!;
    public int ExtraAttributes { get; set; }
    public string? AttributesText { get; set; }

    public IEnumerable<NameCategoryModel> Names { get; set; } = null!;
    public string? NamesText { get; set; }

    public RacialSpeedModel Speeds { get; set; } = null!;
    public string? SpeedsText { get; set; }

    public AgeThresholdsModel? AgeThresholds { get; set; }
    public SizeCategory Size { get; set; }
    public string? StatureRoll { get; set; }
    public WeightRollsModel? WeightRolls { get; set; }
    public string? AgeText { get; set; }
    public string? SizeText { get; set; }
    public string? WeightText { get; set; }

    public IEnumerable<LanguageModel> Languages { get; set; } = null!;
    public int ExtraLanguages { get; set; }
    public string? LanguagesText { get; set; }

    public IEnumerable<RacialTraitModel> Traits { get; set; } = null!;
    public string? TraitsText { get; set; }

    public string? PeopleText { get; set; }
  }
}
