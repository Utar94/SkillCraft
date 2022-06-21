namespace SkillCraft.Core.Characters
{
  public class SkillBonus : BonusBase
  {
    public SkillBonus(Skill skill, Guid userId) : base(userId)
    {
      Skill = skill;
    }

    public Skill Skill { get; private set; }
  }
}
