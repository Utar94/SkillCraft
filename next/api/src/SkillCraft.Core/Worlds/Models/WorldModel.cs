namespace SkillCraft.Core.Worlds.Models
{
  public class WorldModel : AggregateModel
  {
    public string Alias { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
  }
}
