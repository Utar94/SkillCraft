using SkillCraft.Core.Natures;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Gifts
{
  public class Gift : Aggregate
  {
    public Gift(Guid userId, World world) : base(userId)
    {
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Gift() : base()
    {
    }

    public string? Description { get; set; }
    public string Name { get; set; } = null!;
    public int WorldId { get; set; }

    public ICollection<Nature> Natures { get; set; } = new List<Nature>();
    public World? World { get; set; }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
