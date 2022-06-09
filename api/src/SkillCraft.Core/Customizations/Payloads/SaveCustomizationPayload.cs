using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Customizations.Payloads
{
  public abstract class SaveCustomizationPayload
  {
    public string? Description { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
  }
}
