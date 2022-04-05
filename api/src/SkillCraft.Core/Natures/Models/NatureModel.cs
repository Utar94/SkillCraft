using SkillCraft.Core.Models;

namespace SkillCraft.Core.Natures.Models
{
  public class NatureModel : AggregateModel
  {
    public Attribute? Attribute { get; set; }
    public string? Description { get; set; }
    public Guid? GiftId { get; set; }
    public string Name { get; set; } = null!;
  }
}
