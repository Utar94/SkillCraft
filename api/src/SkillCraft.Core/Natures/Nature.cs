using SkillCraft.Core.Gifts;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Natures
{
  public class Nature : Aggregate
  {
    public Nature(Guid userId, World world) : base(userId)
    {
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Nature() : base()
    {
    }

    public Attribute? Attribute { get; set; }
    public string? Description { get; set; }
    public int? GiftId { get; set; }
    public string Name { get; set; } = null!;
    public int WorldId { get; set; }

    public Gift? Gift { get; set; }
    public World? World { get; set; }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
