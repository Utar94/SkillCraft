using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Powers.Payloads
{
  public class DescriptionsPayload
  {
    [StringLength(1000)]
    public string? Global { get; set; }

    [Required]
    [StringLength(1000)]
    public string FirstLevel { get; set; } = null!;

    [Required]
    [StringLength(1000)]
    public string SecondLevel { get; set; } = null!;

    [Required]
    [StringLength(1000)]
    public string ThirdLevel { get; set; } = null!;
  }
}
