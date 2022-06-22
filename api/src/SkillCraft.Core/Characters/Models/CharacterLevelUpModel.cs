namespace SkillCraft.Core.Characters.Models
{
  public class CharacterLevelUpModel
  {
    public int Level { get; set; }

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
