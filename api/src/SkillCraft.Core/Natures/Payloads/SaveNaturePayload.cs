using Logitar.Validation;
using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Natures.Payloads
{
  public abstract class SaveNaturePayload
  {
    public string? Description { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Enum(typeof(Attribute))]
    public Attribute Attribute { get; set; }

    public Guid FeatId { get; set; }
  }
}
