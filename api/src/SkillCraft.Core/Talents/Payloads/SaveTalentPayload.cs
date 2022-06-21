using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Talents.Payloads
{
  public abstract class SaveTalentPayload
  {
    public bool MultipleAcquisition { get; set; }

    public Guid? RequiredTalentId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }
  }
}
