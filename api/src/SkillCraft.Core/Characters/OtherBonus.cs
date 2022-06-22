namespace SkillCraft.Core.Characters
{
  public class OtherBonus : BonusBase
  {
    public OtherBonus(OtherBonusTarget target)
    {
      Target = target;
    }

    public OtherBonusTarget Target { get; private set; }
  }
}
