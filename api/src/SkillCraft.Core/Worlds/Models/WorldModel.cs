using SkillCraft.Core.Models;

namespace SkillCraft.Core.Worlds.Models
{
  public class WorldModel : EntityBaseModel
  {
    public string Alias { get; set; } = null!;
    public string? Description { get; set; }
    public string Name { get; set; } = null!;
  }
}
