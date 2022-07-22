namespace SkillCraft.Core.Talents.Payload
{
  public abstract class SaveTalentPayload
  {
    public Guid? RequiredTalentId { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public bool MultipleAcquisition { get; set; }
    public Skill? Skill { get; set; }

    public IEnumerable<TalentOptionPayload>? Options { get; set; }
  }
}
