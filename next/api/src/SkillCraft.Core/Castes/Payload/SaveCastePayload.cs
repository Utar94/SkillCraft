namespace SkillCraft.Core.Castes.Payload
{
  public abstract class SaveCastePayload
  {
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public Skill Skill { get; set; }
    public string? WealthRoll { get; set; }
  }
}
