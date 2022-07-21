namespace SkillCraft.Core.Educations.Models
{
  public class EducationModel : AggregateModel
  {
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public Skill Skill { get; set; }
    public int WealthMultiplier { get; set; }
  }
}
