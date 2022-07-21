namespace SkillCraft.Core.Worlds.Payload
{
  public class CreateWorldPayload : SaveWorldPayload
  {
    public string Alias { get; set; } = null!;
  }
}
