using SkillCraft.Core.Models;

namespace SkillCraft.Core.Disabilities.Models
{
  public class DisabilityModel : AggregateModel
  {
    public string? Description { get; set; }
    public string Name { get; set; } = null!;
  }
}
