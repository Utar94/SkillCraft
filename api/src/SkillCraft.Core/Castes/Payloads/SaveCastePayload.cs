using Logitar.Validation;
using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Castes.Payloads
{
  public abstract class SaveCastePayload
  {
    public string? Description { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Enum(typeof(Skill))]
    public Skill? Skill { get; set; }

    public IEnumerable<CasteTraitPayload>? Traits { get; set; }

    [RegularExpression("\\dd\\d")]
    public string? WealthRoll { get; set; }
  }
}
