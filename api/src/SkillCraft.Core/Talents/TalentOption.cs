using SkillCraft.Core.Characters;

namespace SkillCraft.Core.Talents
{
  public class TalentOption
  {
    public TalentOption(Talent talent)
    {
      Talent = talent ?? throw new ArgumentNullException(nameof(talent));
      TalentId = talent.Id;
      Uuid = Guid.NewGuid();
    }
    private TalentOption()
    {
    }

    public int Id { get; set; }
    public Guid Uuid { get; set; }

    public Talent? Talent { get; set; }
    public int TalentId { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<CharacterTalent> CharacterTalents { get; set; } = new List<CharacterTalent>();
  }
}
