using Logitar.Validation;
using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Worlds
{
  public class CreateWorldPayload : SaveWorldPayload
  {
    [Required]
    [StringLength(100)]
    [Alias]
    public string Alias { get; set; } = null!;
  }
}
