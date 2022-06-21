using SkillCraft.Core.Characters;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Customizations
{
  public class Customization : EntityBase
  {
    public Customization(CustomizationType type, Guid userId, World world) : base(userId)
    {
      Type = type;
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Customization() : base()
    {
    }

    public World? World { get; set; }
    public int WorldId { get; set; }

    public string? Description { get; set; }
    public string Name { get; set; } = null!;
    public CustomizationType Type { get; set; }

    public ICollection<Character> Characters { get; set; } = new List<Character>();

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
