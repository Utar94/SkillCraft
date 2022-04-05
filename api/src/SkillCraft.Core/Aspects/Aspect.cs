using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Aspects
{
  public class Aspect : Aggregate
  {
    public Aspect(Guid userId, World world) : base(userId)
    {
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Aspect() : base()
    {
    }

    public string? Description { get; set; }
    public string Name { get; set; } = null!;
    public int WorldId { get; set; }

    public Attribute? MandatoryAttribute1 { get; set; }
    public Attribute? MandatoryAttribute2 { get; set; }
    public Attribute? OptionalAttribute1 { get; set; }
    public Attribute? OptionalAttribute2 { get; set; }

    public Skill? Skill1 { get; set; }
    public Skill? Skill2 { get; set; }

    public World? World { get; set; }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
