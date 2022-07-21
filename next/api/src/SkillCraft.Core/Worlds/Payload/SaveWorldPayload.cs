namespace SkillCraft.Core.Worlds.Payload
{
  public abstract class SaveWorldPayload
  {
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
  }
}
