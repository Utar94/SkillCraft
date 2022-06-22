using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Characters.Payloads
{
  public class SaveCharacterStep2Payload
  {
    public Guid Aspect1Id { get; set; }

    public Guid Aspect2Id { get; set; }

    [Required]
    public CharacterCreationPayload Creation { get; set; } = null!;

    public Guid RaceId { get; set; }

    [Range(0, 99.99)]
    public double Stature { get; set; }

    [Range(0, 999.9)]
    public double Weight { get; set; }

    [Range(0, 9999)]
    public int Age { get; set; }

    public IEnumerable<Guid>? LanguageIds { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    public IEnumerable<BonusPayload>? Bonuses { get; set; }
  }
}
