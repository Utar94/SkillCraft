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
    public Character(Guid userId, World world) : base(userId)
    {
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Character() : base()
    {
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
    public ICollection<CharacterTalent> Talents { get; set; } = new List<CharacterTalent>();

    // TODO(fpion): Powers (n..n)
    // TODO(fpion): Inventory (n..n)*
    // TODO(fpion): Attacks & Defense (JSON & computed)
    // TODO(fpion): Notes (1..n)

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
