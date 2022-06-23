using Logitar.Validation;
using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Talents.Payloads
{
  public abstract class SaveTalentPayload
  {
    public bool MultipleAcquisition { get; set; }

    public Guid? RequiredTalentId { get; set; }

    [Enum(typeof(Skill))]
    public Skill? Skill { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public IEnumerable<TalentOptionPayload>? Options { get; set; }
  }
}
