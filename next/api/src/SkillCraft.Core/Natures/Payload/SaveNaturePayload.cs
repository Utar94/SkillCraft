namespace SkillCraft.Core.Natures.Payload
{
  public abstract class SaveNaturePayload
  {
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public Attribute Attribute { get; set; }
    public Guid? FeatId { get; set; }
  }
}
