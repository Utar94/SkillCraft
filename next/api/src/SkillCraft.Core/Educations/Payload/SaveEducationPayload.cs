namespace SkillCraft.Core.Educations.Payload
{
  public abstract class SaveEducationPayload
  {
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public Skill Skill { get; set; }
    public int WealthMultiplier { get; set; }
  }
}
