namespace SkillCraft.Core.Characters
{
  public class CharacterLevelUp
  {
    public CharacterLevelUp(Guid userId)
    {
      PerformedAt = DateTime.UtcNow;
      PerformedById = userId;
    }
    private CharacterLevelUp()
    {
    }

    public DateTime PerformedAt { get; set; }
    public Guid PerformedById { get; set; }

    public Attribute Attribute { get; set; }

    public int Constitution { get; set; }
    public double Initiative { get; set; }
    public int Learning { get; set; }
    public double Power { get; set; }
    public double Precision { get; set; }
    public double Repute { get; set; }
    public double Strength { get; set; }
  }
}
