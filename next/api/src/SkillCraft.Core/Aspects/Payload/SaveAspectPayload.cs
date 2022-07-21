namespace SkillCraft.Core.Aspects.Payload
{
  public abstract class SaveAspectPayload
  {
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public Attribute MandatoryAttribute1 { get; set; }
    public Attribute MandatoryAttribute2 { get; set; }
    public Attribute OptionalAttribute1 { get; set; }
    public Attribute OptionalAttribute2 { get; set; }

    public Skill Skill1 { get; set; }
    public Skill Skill2 { get; set; }
  }
}
