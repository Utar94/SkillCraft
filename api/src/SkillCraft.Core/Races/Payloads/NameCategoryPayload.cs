using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Races.Payloads
{
  public class NameCategoryPayload
  {
    [Required]
    [StringLength(20)]
    public string Category { get; set; } = null!;

    public IEnumerable<string> Values { get; set; } = null!;
  }
}
