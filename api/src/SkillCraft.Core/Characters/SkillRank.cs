namespace SkillCraft.Core.Characters
{
  public class SkillRank
  {
    public SkillRank(Skill skill)
    {
      Id = Guid.NewGuid();
      Skill = skill;
    }
    private SkillRank()
    {
    }

    public Guid Id { get; set; }

    public Skill Skill { get; private set; }
    public bool Training { get; set; }
  }
}
