using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Conditions.Payloads
{
  public abstract class SaveConditionPayload
  {
    public string? Description { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Range(0, 6)]
    public int MaxLevel { get; set; }
  }
}
