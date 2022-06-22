namespace SkillCraft.Core.Characters.Models
{
  public class BonusModel
  {
    public Guid Id { get; set; }

    public BonusType Type { get; set; }
    public string Target { get; set; } = null!;

    public string? Description { get; set; }
    public bool Permanent { get; set; }
    public int Value { get; set; }
  }
}
