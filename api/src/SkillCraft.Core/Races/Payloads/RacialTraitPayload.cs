using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Races.Payloads
{
  public class RacialTraitPayload
  {
    public Guid? Id { get; set; }

    public string? Description { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
  }
}
