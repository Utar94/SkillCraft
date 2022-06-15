using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Educations
{
  public class Education : EntityBase
  {
    public Education(Guid userId, World world) : base(userId)
    {
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Education() : base()
    {
    }

    public World? World { get; set; }
    public int WorldId { get; set; }

    public string? Description { get; set; }
    public string Name { get; set; } = null!;

    public Skill Skill { get; set; }
    public int? WealthMultiplier { get; set; }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
