using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Castes
{
  public class Caste : EntityBase
  {
    public Caste(Guid userId, World world) : base(userId)
    {
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Caste() : base()
    {
    }

    public World? World { get; set; }
    public int WorldId { get; set; }

    public string? Description { get; set; }
    public string Name { get; set; } = null!;

    public Skill Skill { get; set; }
    public string? WealthRoll { get; set; }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
