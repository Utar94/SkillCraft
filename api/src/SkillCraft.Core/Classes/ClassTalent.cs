using SkillCraft.Core.Talents;

namespace SkillCraft.Core.Classes
{
  public class ClassTalent
  {
    public ClassTalent(Class @class, Talent talent)
    {
      Class = @class ?? throw new ArgumentNullException(nameof(@class));
      ClassId = @class.Id;
      Talent = talent ?? throw new ArgumentNullException(nameof(talent));
      TalentId = talent.Id;
    }
    private ClassTalent()
    {
    }

    public Class? Class { get; set; }
    public int ClassId { get; set; }

    public Talent? Talent { get; set; }
    public int TalentId { get; set; }

    public bool Mandatory { get; set; }
  }
}
