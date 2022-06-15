using SkillCraft.Core.Models;

namespace SkillCraft.Core.Educations.Models
{
  public class EducationModel : EntityBaseModel
  {
    public string? Description { get; set; }
    public string Name { get; set; } = null!;

    public Skill Skill { get; set; }
    public int? WealthMultiplier { get; set; }
  }
}
