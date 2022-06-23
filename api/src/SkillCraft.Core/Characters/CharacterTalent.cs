using SkillCraft.Core.Talents;

namespace SkillCraft.Core.Characters
{
  public class CharacterTalent
  {
    public CharacterTalent(Character character, Talent talent)
    {
      Character = character ?? throw new ArgumentNullException(nameof(character));
      CharacterId = character.Id;
      Talent = talent ?? throw new ArgumentNullException(nameof(talent));
      TalentId = talent.Id;
      Uuid = Guid.NewGuid();
    }
    private CharacterTalent()
    {
    }

    public int Id { get; set; }
    public Guid Uuid { get; set; }

    public Character? Character { get; set; }
    public int CharacterId { get; set; }
    public Talent? Talent { get; set; }
    public int TalentId { get; set; }
    public TalentOption? Option { get; set; }
    public int? OptionId { get; set; }

    public int Cost { get; set; }
    public string? Description { get; set; }
  }
}
