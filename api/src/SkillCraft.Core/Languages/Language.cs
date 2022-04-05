using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Languages
{
  public class Language : Aggregate
  {
    public Language(Guid userId, World world) : base(userId)
    {
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Language() : base()
    {
    }

    public string? Description { get; set; }
    public bool Exotic { get; set; }
    public string Name { get; set; } = null!;
    public string? Script { get; set; }
    public string? TypicalSpeakers { get; set; }
    public int WorldId { get; set; }

    public World? World { get; set; }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
