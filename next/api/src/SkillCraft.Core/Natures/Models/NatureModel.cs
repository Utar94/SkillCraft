using SkillCraft.Core.Customizations.Models;

namespace SkillCraft.Core.Natures.Models
{
  public class NatureModel : AggregateModel
  {
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public Attribute Attribute { get; set; }
    public CustomizationModel? Feat { get; set; }
  }
}
