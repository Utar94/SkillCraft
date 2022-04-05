using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Gifts.Payloads
{
  public abstract class SaveGiftPayload
  {
    public string? Description { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
  }
}
