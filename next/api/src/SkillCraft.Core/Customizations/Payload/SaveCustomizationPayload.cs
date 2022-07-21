namespace SkillCraft.Core.Customizations.Payload
{
  public abstract class SaveCustomizationPayload
  {
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
  }
}
