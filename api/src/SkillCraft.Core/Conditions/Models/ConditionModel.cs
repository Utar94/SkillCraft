using SkillCraft.Core.Models;

namespace SkillCraft.Core.Conditions.Models
{
  public class ConditionModel : EntityBaseModel
  {
    public string? Description { get; set; }
    public string Name { get; set; } = null!;

    public int MaxLevel { get; set; }
  }
}
