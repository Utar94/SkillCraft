namespace SkillCraft.Core.Castes.Models
{
  public class CasteModel : AggregateModel
  {
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public Skill Skill { get; set; }
    public string? WealthRoll { get; set; }
  }
}
