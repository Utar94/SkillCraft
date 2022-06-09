using SkillCraft.Core.Models;

namespace SkillCraft.Core.Customizations.Models
{
  public class CustomizationModel : EntityBaseModel
  {
    public string? Description { get; set; }
    public string Name { get; set; } = null!;
    public CustomizationType Type { get; set; }
  }
}
