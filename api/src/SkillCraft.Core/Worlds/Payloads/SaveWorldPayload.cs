using Logitar.Validation;
using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Worlds.Payloads
{
  public abstract class SaveWorldPayload
  {
    [Enum(typeof(Confidentiality))]
    public Confidentiality Confidentiality { get; set; }

    public string? Description { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
  }
}
