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
    public Skill Skill { get; set; }

    [RegularExpression("\\d{1,2}d\\d{1,2}(\\+\\d{1,3})?")] // TODO(fpion): refactor
    public string? WealthRoll { get; set; }
  }
}
