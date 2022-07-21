namespace SkillCraft.Core.Languages.Payload
{
  public abstract class SaveLanguagePayload
  {
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public bool IsExotic { get; set; }
    public string? Script { get; set; }
    public string? TypicalSpeakers { get; set; }
  }
}
