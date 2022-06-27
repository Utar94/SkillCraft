using SkillCraft.Core.Aspects;
using SkillCraft.Core.Castes;
using SkillCraft.Core.Customizations;
using SkillCraft.Core.Educations;
using SkillCraft.Core.Languages;
using SkillCraft.Core.Natures;
using SkillCraft.Core.Races;
using SkillCraft.Core.Worlds;
using System.Text.Json;

namespace SkillCraft.Core.Characters
{
  public class Character : EntityBase
  {
    private readonly ExperienceTable _experienceTable = new();

    public Character(Guid userId, World world) : base(userId)
    {
      Attributes = new(this);
      Statistics = new(this);
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Character() : base()
    {
      Attributes = new(this);
      Statistics = new(this);
    }

    public World? World { get; set; }
    public int WorldId { get; set; }

    public string Name { get; set; } = null!;
    public string? Player { get; set; }

    public Aspect? Aspect1 { get; set; }
    public int? Aspect1Id { get; set; }
    public Aspect? Aspect2 { get; set; }
    public int? Aspect2Id { get; set; }
    public Caste? Caste { get; set; }
    public int? CasteId { get; set; }
    public Education? Education { get; set; }
    public int? EducationId { get; set; }
    public Nature? Nature { get; set; }
    public int? NatureId { get; set; }
    public Race? Race { get; set; }
    public int? RaceId { get; set; }

    public double Stature { get; set; }
    public double Weight { get; set; }
    public int Age { get; set; }

    public int Experience { get; set; }
    public int Vitality { get; set; }
    public int Stamina { get; set; }

    public int BloodAlcoholContent { get; set; }
    public int Intoxication { get; set; }

    public string? Description { get; set; }

    public List<BonusBase> Bonuses { get; set; } = new();
    public string[]? BonusesSerialized
    {
      get => Bonuses.Any() ? Bonuses.Select(bonus => bonus.Serialize()).ToArray() : null;
      set
      {
        Bonuses.Clear();

        if (value != null)
        {
          Bonuses = value.Select(json => BonusBase.Deserialize(json)).ToList();
        }
      }
    }

    public CharacterCreation? Creation { get; set; }
    public string? CreationSerialized
    {
      get => Creation == null ? null : JsonSerializer.Serialize(Creation);
      set => Creation = value == null ? null : JsonSerializer.Deserialize<CharacterCreation>(value);
    }

    public Dictionary<int, CharacterLevelUp> LevelUps { get; set; } = new();
    public string? LevelUpsSerialized
    {
      get => LevelUps.Any() ? JsonSerializer.Serialize(LevelUps) : null;
      set
      {
        LevelUps.Clear();

        if (value != null)
        {
          LevelUps = JsonSerializer.Deserialize<Dictionary<int, CharacterLevelUp>>(value) ?? new();
        }
      }
    }

    public List<SkillRank> SkillRanks { get; set; } = new();
    public string? SkillRanksSerialized
    {
      get => SkillRanks.Any() ? JsonSerializer.Serialize(SkillRanks) : null;
      set
      {
        SkillRanks.Clear();

        if (value != null)
        {
          SkillRanks = JsonSerializer.Deserialize<List<SkillRank>>(value) ?? new();
        }
      }
    }

    public ICollection<CharacterCondition> Conditions { get; set; } = new List<CharacterCondition>();
    public ICollection<Customization> Customizations { get; set; } = new List<Customization>();
    public ICollection<Language> Languages { get; set; } = new List<Language>();
    public ICollection<CharacterPower> Powers { get; set; } = new List<CharacterPower>();
    public ICollection<CharacterTalent> Talents { get; set; } = new List<CharacterTalent>();

    public int Level => _experienceTable.GetLevel(Experience);
    public int MaxVitality
    {
      get
      {
        int value = Statistics.Constitution.Value;

        foreach (BonusBase bonus in Bonuses)
        {
          if (bonus is OtherBonus otherBonus && otherBonus.Target == OtherBonusTarget.Vitality)
          {
            value += otherBonus.Value;
          }
        }

        return value;
      }
    }
    public int MaxStamina
    {
      get
      {
        int value = Statistics.Constitution.Value;

        foreach (BonusBase bonus in Bonuses)
        {
          if (bonus is OtherBonus otherBonus && otherBonus.Target == OtherBonusTarget.Stamina)
          {
            value += otherBonus.Value;
          }
        }

        return value;
      }
    }

    public CharacterAttributes Attributes { get; }
    public CharacterStatistics Statistics { get; }

    public int RemainingLearningPoints => TotalLearningPoints - SpentLearningPoints;
    public int SpentLearningPoints => SkillRanks.Sum(x => x.Cost);
    public int TotalLearningPoints => Statistics.Learning.Value;

    public int RemainingTalentPoints => TotalTalentPoints - SpentTalentPoints;
    public int SpentTalentPoints => Talents.Sum(x => x.Cost) + Powers.Sum(x => x.Cost);
    public int TotalTalentPoints => (Level + 1) * 4;

    // TODO(fpion): Inventory (n..n)*
    // TODO(fpion): Attacks & Defense (JSON & computed)
    // TODO(fpion): Notes (1..n)

    public override string ToString() => $"{Name} | {base.ToString()}";

    public CharacterLevelUp LevelUp(Attribute attribute)
    {
      int level = (LevelUps.Any() ? LevelUps.Max(x => x.Key) : 0) + 1;

      if (level > Level)
      {
        throw new InvalidOperationException("The character cannot level-up.");
      }

      var levelUp = new CharacterLevelUp(attribute);
      LevelUps.Add(level, levelUp);

      levelUp.CalculateStatistics(Statistics);

      return levelUp;
    }

    public void Validate()
    {
      if (RemainingLearningPoints < 0)
      {
        throw new SpentLearningPointsExceededException(this);
      }

      if (RemainingTalentPoints < 0)
      {
        throw new SpentTalentPointsExceededException(this);
      }
    }
  }
}
