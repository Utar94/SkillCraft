namespace SkillCraft.Core.Races.Payload
{
  public class NameCategoryPayload
  {
    public string Category { get; set; } = null!;
    public IEnumerable<string> Values { get; set; } = null!;
  }
}
