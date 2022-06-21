using SkillCraft.Core.Characters;
using SkillCraft.Core.Customizations;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Natures
{
  public class Nature : EntityBase
  {
    public Nature(Guid userId, World world) : base(userId)
    {
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Nature() : base()
    {
    }

    public World? World { get; set; }
    public int WorldId { get; set; }

    public string? Description { get; set; }
    public string Name { get; set; } = null!;

    public Attribute Attribute { get; set; }
    public Customization? Feat { get; set; }
    public int FeatId { get; set; }

    public ICollection<Character> Characters { get; set; } = new List<Character>();

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
