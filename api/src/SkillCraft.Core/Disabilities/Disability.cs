using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Disabilities
{
  public class Disability : Aggregate
  {
    public Disability(Guid userId, World world) : base(userId)
    {
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Disability() : base()
    {
    }

    public string? Description { get; set; }
    public string Name { get; set; } = null!;
    public int WorldId { get; set; }

    public World? World { get; set; }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
