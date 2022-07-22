namespace SkillCraft.Core.Races.Models
{
  public class NameCategoryModel
  {
    public string Category { get; set; } = null!;
    public IEnumerable<string> Values { get; set; } = null!;
  }
}
