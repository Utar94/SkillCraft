namespace SkillCraft.Core.Characters
{
  public class CharacterLevelUp
  {
    public CharacterLevelUp(Attribute attribute)
    {
      Attribute = attribute;
    }
    private CharacterLevelUp()
    {
    }

    public Attribute Attribute { get; private set; }

    public int Constitution { get; set; }
    public double Initiative { get; set; }
    public int Learning { get; set; }
    public double Power { get; set; }
    public double Precision { get; set; }
    public double Repute { get; set; }
    public double Strength { get; set; }
  }
}
