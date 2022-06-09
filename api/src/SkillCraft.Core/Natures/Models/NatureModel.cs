using SkillCraft.Core.Customizations.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Natures.Models
{
  public class NatureModel : EntityBaseModel
  {
    public string? Description { get; set; }
    public string Name { get; set; } = null!;

    public Attribute Attribute { get; set; }
    public CustomizationModel? Feat { get; set; }
  }
}
