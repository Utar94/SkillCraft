namespace SkillCraft.Core.Characters
{
  public class LevelUp
  {
    public LevelUp(Character character, int level, Guid userId)
    {
      Character = character ?? throw new ArgumentNullException(nameof(character));
      CharacterId = character.Id;
      Level = level;
      PerformedAt = DateTime.UtcNow;
      PerformedById = userId;
    }
    private LevelUp()
    {
    }

    public Character? Character { get; set; }
    public int CharacterId { get; set; }
    public int Level { get; set; }

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
