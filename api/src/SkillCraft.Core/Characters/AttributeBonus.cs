namespace SkillCraft.Core.Characters
{
  public class AttributeBonus : BonusBase
  {
    public AttributeBonus(Attribute attribute, Guid userId) : base(userId)
    {
      Attribute = attribute;
    }

    public Attribute Attribute { get; private set; }
  }
}
