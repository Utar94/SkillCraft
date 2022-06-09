using Logitar;
using SkillCraft.Core.Aspects;
using SkillCraft.Core.Customizations;

namespace SkillCraft.Core.Worlds
{
  public class World : EntityBase
  {
    public World(string alias, Guid userId) : base(userId)
    {
      Alias = alias?.CleanTrim()?.ToLowerInvariant() ?? throw new ArgumentNullException(nameof(alias));
    }
    private World() : base()
    {
    }

    public string Alias { get; set; } = null!;
    public string? Description { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Aspect> Aspects { get; set; } = new List<Aspect>();
    public ICollection<Customization> Customizations { get; set; } = new List<Customization>();

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
