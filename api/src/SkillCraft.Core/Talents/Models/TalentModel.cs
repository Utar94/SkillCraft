using SkillCraft.Core.Models;

namespace SkillCraft.Core.Talents.Models
{
  public class TalentModel : AggregateModel
  {
    public string? Description { get; set; }
    public bool MultipleAcquisitions { get; set; }
    public string Name { get; set; } = null!;
    public Guid? RequiredTalentId { get; set; }
    public int Tier { get; set; }
  }
}
