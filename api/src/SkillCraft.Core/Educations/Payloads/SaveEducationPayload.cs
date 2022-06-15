using Logitar.Validation;
using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Educations.Payloads
{
  public abstract class SaveEducationPayload
  {
    public string? Description { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Enum(typeof(Skill))]
    public Skill Skill { get; set; }

    [Range(1, 12)]
    public int? WealthMultiplier { get; set; }
  }
}
