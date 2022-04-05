using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Castes.Payloads
{
  public class CasteTraitPayload
  {
    public string? Description { get; set; }

    public Guid? Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
  }
}
