using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Talents
{
  public class Talent : Aggregate
  {
    public Talent(Guid userId, World world) : base(userId)
    {
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Talent() : base()
    {
    }

    public string? Description { get; set; }
    public bool MultipleAcquisitions { get; set; }
    public string Name { get; set; } = null!;
    public int? RequiredTalentId { get; set; }
    public int Tier { get; set; }
    public int WorldId { get; set; }

    public Talent? RequiredTalent { get; set; }
    public ICollection<Talent> RequiringTalents { get; set; } = new List<Talent>();
    public World? World { get; set; }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
