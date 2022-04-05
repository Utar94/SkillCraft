using SkillCraft.Core.Models;

namespace SkillCraft.Core.Worlds.Models
{
  public class WorldModel : AggregateModel
  {
    public string Alias { get; set; } = null!;
    public Confidentiality Confidentiality { get; set; }
    public string? Description { get; set; }
    public string Name { get; set; } = null!;
  }
}
