using SkillCraft.Core.Characters;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Powers
{
  public class Power : EntityBase
  {
    public Power(int tier, Guid userId, World world) : base(userId)
    {
      Tier = tier;
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Power() : base()
    {
    }

    public World? World { get; set; }
    public int WorldId { get; set; }

    public int Tier { get; set; }
    public int Cost => Tier + 2;

    public string Name { get; set; } = null!;
    public string[]? Descriptions { get; set; }

    public IncantationType Incantation { get; set; }
    public int? Duration { get; set; }
    public int? Range { get; set; }
    public string? Ingredients { get; set; }

    public bool Concentration { get; set; }
    public bool Focus { get; set; }
    public bool Ritual { get; set; }
    public bool Somatic { get; set; }
    public bool Verbal { get; set; }

    public ICollection<CharacterPower> CharacterPowers { get; set; } = new List<CharacterPower>();

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
