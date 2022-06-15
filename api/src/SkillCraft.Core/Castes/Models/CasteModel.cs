using SkillCraft.Core.Models;

namespace SkillCraft.Core.Castes.Models
{
  public class CasteModel : EntityBaseModel
  {
    public string? Description { get; set; }
    public string Name { get; set; } = null!;

    public Skill Skill { get; set; }
    public string? WealthRoll { get; set; }
  }
}
