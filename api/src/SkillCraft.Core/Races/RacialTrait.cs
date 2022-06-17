namespace SkillCraft.Core.Races
{
  public class RacialTrait
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
  }
}
