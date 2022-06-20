using SkillCraft.Core.Aspects;
using SkillCraft.Core.Castes;
using SkillCraft.Core.Educations;
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
    public Race? Race { get; set; }
    public int? RaceId { get; set; }
    public Nature? Nature { get; set; }
    public int? NatureId { get; set; }
    public Caste? Caste { get; set; }
    public int? CasteId { get; set; }
    public Education? Education { get; set; }
    public int? EducationId { get; set; }

    public SizeCategory Size { get; set; }
    public double? Stature { get; set; }
    public double? Weight { get; set; }
    public int? Age { get; set; }

    public int Experience { get; set; }
    public int Vitality { get; set; }
    public int Stamina { get; set; }

    public int BloodAlcoholContent { get; set; }
    public int Intoxication { get; set; }

    public string? Description { get; set; }

    public Creation? Creation { get; set; }
    public string? CreationSerialized
    {
      get => Creation == null ? null : JsonSerializer.Serialize(Creation);
      set => Creation = value == null ? null : JsonSerializer.Deserialize<Creation>(value);
    }

    public ICollection<LevelUp> LevelUps { get; set; } = new List<LevelUp>(); // (❓) => JSON

    // TODO(fpion): Customizations (Feats & Disabilities) (n..n)
    // TODO(fpion): Languages (n..n)
    // TODO(fpion): Skills (❓) => JSON
    // TODO(fpion): Bonuses (❓) => JSON
    // TODO(fpion): Conditions (string[]?)
    // TODO(fpion): Talents & Powers (n..n)
    // TODO(fpion): Inventory (n..n)*
    // TODO(fpion): Attacks & Defense (JSON & computed)
    // TODO(fpion): Notes (1..n)
  }
}
