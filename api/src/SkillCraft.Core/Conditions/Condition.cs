using SkillCraft.Core.Characters;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Conditions
{
  public class Condition : EntityBase
  {
    public Condition(Guid userId, World world) : base(userId)
    {
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Condition() : base()
    {
    }

    public World? World { get; set; }
    public int WorldId { get; set; }

    public string? Description { get; set; }
    public string Name { get; set; } = null!;

    public int MaxLevel { get; set; }

    public ICollection<CharacterCondition> CharacterConditions { get; set; } = new List<CharacterCondition>();

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
