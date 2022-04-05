using Logitar.Validation;
using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Natures.Payloads
{
  public abstract class SaveNaturePayload
  {
    [Enum(typeof(Attribute))]
    public Attribute? Attribute { get; set; }

    public string? Description { get; set; }

    public Guid? GiftId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
  }
}
