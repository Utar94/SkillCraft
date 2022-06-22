namespace SkillCraft.Core.Characters
{
  public class AttributeBonus : BonusBase
  {
    public AttributeBonus(Attribute attribute)
    {
      Attribute = attribute;
    }

    public Attribute Attribute { get; private set; }
  }
}
