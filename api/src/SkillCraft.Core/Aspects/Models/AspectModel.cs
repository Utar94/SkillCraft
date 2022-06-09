using SkillCraft.Core.Models;

namespace SkillCraft.Core.Aspects.Models
{
  public class AspectModel : EntityBaseModel
  {
    public string? Description { get; set; }
    public string Name { get; set; } = null!;

    public Attribute MandatoryAttribute1 { get; set; }
    public Attribute MandatoryAttribute2 { get; set; }
    public Attribute OptionalAttribute1 { get; set; }
    public Attribute OptionalAttribute2 { get; set; }
    public Skill Skill1 { get; set; }
    public Skill Skill2 { get; set; }
  }
}
