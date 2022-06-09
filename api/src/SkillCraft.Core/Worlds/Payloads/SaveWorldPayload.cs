using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Worlds.Payloads
{
  public abstract class SaveWorldPayload
  {
    public string? Description { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
  }
}
