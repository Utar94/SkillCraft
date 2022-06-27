using SkillCraft.Core.Powers;

namespace SkillCraft.Core.Characters
{
  public class CharacterPower
  {
    public CharacterPower(Character character, Power power)
    {
      Character = character ?? throw new ArgumentNullException(nameof(character));
      CharacterId = character.Id;
      Power = power ?? throw new ArgumentNullException(nameof(power));
      PowerId = power.Id;
      Uuid = Guid.NewGuid();
    }
    private CharacterPower()
    {
    }

    public int Id { get; set; }
    public Guid Uuid { get; set; }

    public Character? Character { get; set; }
    public int CharacterId { get; set; }
    public Power? Power { get; set; }
    public int PowerId { get; set; }

    public int Cost { get; set; }
    public string? Description { get; set; }
  }
}
