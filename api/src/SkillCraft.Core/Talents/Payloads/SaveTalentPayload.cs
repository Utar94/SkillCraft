using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Talents.Payloads
{
  public abstract class SaveTalentPayload
  {
    public string? Description { get; set; }

    public bool MultipleAcquisitions { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    public Guid? RequiredTalentId { get; set; }

    [Range(0, 3)]
    public int Tier { get; set; }
  }
}
