namespace SkillCraft.Core.Characters
{
  public class OtherBonus : BonusBase
  {
    public OtherBonus(OtherBonusTarget target, Guid userId) : base(userId)
    {
      Target = target;
    }

    public OtherBonusTarget Target { get; private set; }
  }
}
