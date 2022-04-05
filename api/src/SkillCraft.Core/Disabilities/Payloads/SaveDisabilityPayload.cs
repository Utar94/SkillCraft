using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Disabilities.Payloads
{
  public abstract class SaveDisabilityPayload
  {
    public string? Description { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
  }
}
