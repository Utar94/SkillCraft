using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Talents
{
  public class Talent : EntityBase
  {
    public Talent(int tier, Guid userId, World world) : base(userId)
    {
      Tier = tier;
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Talent() : base()
    {
    }

    public World? World { get; set; }
    public int WorldId { get; set; }

    public bool MultipleAcquisition { get; set; }
    public Talent? RequiredTalent { get; set; }
    public int? RequiredTalentId { get; set; }
    public int Tier { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<Talent> RequiringTalents { get; set; } = new List<Talent>();

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
