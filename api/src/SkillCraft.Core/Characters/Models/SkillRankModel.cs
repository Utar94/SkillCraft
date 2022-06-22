namespace SkillCraft.Core.Characters.Models
{
  public class SkillRankModel
  {
    public Guid Id { get; set; }

    public Skill Skill { get; set; }
    public bool Training { get; set; }
  }
}
