using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Talents.Payloads
{
  public class TalentOptionPayload
  {
    public Guid? Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }
  }
}
