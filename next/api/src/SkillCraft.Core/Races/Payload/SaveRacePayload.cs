namespace SkillCraft.Core.Races.Payload
{
  public abstract class SaveRacePayload
  {
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public AttributeBonusesPayload? Attributes { get; set; }
    public int ExtraAttributes { get; set; }
    public string? AttributesText { get; set; }

    public IEnumerable<NameCategoryPayload>? Names { get; set; }
    public string? NamesText { get; set; }

    public RacialSpeedPayload? Speeds { get; set; }
    public string? SpeedsText { get; set; }

    public AgeThresholdsPayload? AgeThresholds { get; set; }
    public SizeCategory Size { get; set; }
    public string? StatureRoll { get; set; }
    public WeightRollsPayload? WeightRolls { get; set; }
    public string? AgeText { get; set; }
    public string? SizeText { get; set; }
    public string? WeightText { get; set; }

    public IEnumerable<Guid>? LanguageIds { get; set; }
    public int ExtraLanguages { get; set; }
    public string? LanguagesText { get; set; }

    public IEnumerable<RacialTraitPayload>? Traits { get; set; }
    public string? TraitsText { get; set; }

    public string? PeopleText { get; set; }
  }
}
