namespace SkillCraft.Core.Talents.Models
{
  public class TalentOptionModel
  {
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
  }
}
