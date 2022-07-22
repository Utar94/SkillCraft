using Logitar;

namespace SkillCraft.Core.Talents
{
  public class TalentOption
  {
    public TalentOption(Talent talent, string name, string? description = null)
    {
      Id = Guid.NewGuid();

      Talent = talent ?? throw new ArgumentNullException(nameof(talent));
      TalentSid = talent.Sid;

      Update(name, description);
    }
    private TalentOption()
    {
    }

    public Guid Id { get; private set; }
    public int Sid { get; private set; }

    public Talent? Talent { get; private set; }
    public int TalentSid { get; private set; }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public void Update(string name, string? description)
    {
      Name = name?.Trim() ?? throw new ArgumentNullException(nameof(name));
      Description = description?.CleanTrim();
    }
  }
}
