using SkillCraft.Core.Models;

namespace SkillCraft.Core.Castes.Models
{
  public class CasteModel : AggregateModel
  {
    public string? Description { get; set; }
    public string Name { get; set; } = null!;
    public Skill? Skill { get; set; }
    public IEnumerable<CasteTraitModel> Traits { get; set; } = null!;
    public string? WealthRoll { get; set; }
  }
}
