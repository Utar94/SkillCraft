namespace SkillCraft.Core.Characters
{
  public class SkillRank
  {
    public SkillRank(Skill skill, bool training)
    {
      Id = Guid.NewGuid();
      Skill = skill;
      Training = training;
    }
    private SkillRank()
    {
    }

    public Guid Id { get; set; }

    public Skill Skill { get; private set; }
    public bool Training { get; private set; }

    public int Cost => Training ? 1 : 2;
  }
}
