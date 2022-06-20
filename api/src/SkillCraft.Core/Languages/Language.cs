using SkillCraft.Core.Races;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Languages
{
  public class Language : EntityBase
  {
    public Language(Guid userId, World world) : base(userId)
    {
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Language() : base()
    {
    }

    public World? World { get; set; }
    public int WorldId { get; set; }

    public string? Description { get; set; }
    public string Name { get; set; } = null!;

    public bool Exotic { get; set; }
    public string? Script { get; set; }
    public string? TypicalSpeakers { get; set; }

    public ICollection<Race> Races { get; set; } = new List<Race>();

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
