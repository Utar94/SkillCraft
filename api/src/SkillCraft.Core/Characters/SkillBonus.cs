namespace SkillCraft.Core.Characters
{
  public class SkillBonus : BonusBase
  {
    public SkillBonus(Skill skill)
    {
      Skill = skill;
    }

    public Skill Skill { get; private set; }
  }
}
