using Logitar.Validation;
using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Characters.Payloads
{
  public class CreateCharacterPayload
  {
    #region Step 1
    public Guid? Aspect1Id { get; set; }

    public Guid? Aspect2Id { get; set; }

    [Required]
    public CharacterCreationPayload Creation { get; set; } = null!;
    #endregion

    #region Step 2
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    public Guid? RaceId { get; set; }

    [Enum(typeof(SizeCategory))]
    public SizeCategory Size { get; set; }

    [Range(0, 99.99)]
    public double Stature { get; set; }

    [Range(0, 999.9)]
    public double Weight { get; set; }

    [Range(0, 999)]
    public int Age { get; set; }

    public IEnumerable<Guid>? LanguageIds { get; set; }
    #endregion

    #region Step 3
    public Guid? NatureId { get; set; }

    // TODO(fpion): Customizations
    #endregion

    #region Step 4
    public Guid? CasteId { get; set; }

    public Guid? EducationId { get; set; }

    // TODO(fpion): Talents & Powers

    public string? Description { get; set; }

    public IEnumerable<CreateSkillRankPayload>? SkillRanks { get; set; }
    #endregion

    #region Step 5
    // TODO(fpion): Inventory
    #endregion

    public IEnumerable<CreateBonusPayload>? Bonuses { get; set; }
  }
}
