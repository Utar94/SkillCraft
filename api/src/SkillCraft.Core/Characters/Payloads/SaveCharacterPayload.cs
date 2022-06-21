using Logitar.Validation;
using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Characters.Payloads
{
  public class SaveCharacterPayload
  {
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string? Player { get; set; }

    public Guid? Aspect1Id { get; set; }

    public Guid? Aspect2Id { get; set; }

    public Guid? RaceId { get; set; }

    public Guid? NatureId { get; set; }

    public Guid? CasteId { get; set; }

    public Guid? EducationId { get; set; }

    [Enum(typeof(SizeCategory))]
    public SizeCategory Size { get; set; }

    [Range(0, 999.99)]
    public double Stature { get; set; }

    [Range(0, 999.99)]
    public double Weight { get; set; }

    [Range(0, 999)]
    public int Age { get; set; }

    [Range(0, 999999)]
    public int Experience { get; set; }

    [Range(0, 999)]
    public int Vitality { get; set; }

    [Range(0, 999)]
    public int Stamina { get; set; }

    [Range(0, 99)]
    public int BloodAlcoholContent { get; set; }

    [Range(0, 99)]
    public int Intoxication { get; set; }

    public string? Description { get; set; }

    public CharacterCreationPayload? Creation { get; set; }

    public IEnumerable<SkillRankPayload>? SkillRanks { get; set; }

    public IEnumerable<BonusPayload>? Bonuses { get; set; }

    public IEnumerable<CharacterConditionPayload>? Conditions { get; set; }

    public IEnumerable<Guid>? CustomizationIds { get; set; }

    public IEnumerable<Guid>? LanguageIds { get; set; }

    public IEnumerable<CharacterTalentPayload>? Talents { get; set; }
  }
}
