using SkillCraft.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Races.Payloads
{
  public class WeightRollsPayload
  {
    [Required]
    [Roll]
    public string Skinny { get; set; } = null!;

    [Required]
    [Roll]
    public string Thin { get; set; } = null!;

    [Required]
    [Roll]
    public string Normal { get; set; } = null!;

    [Required]
    [Roll]
    public string Overweight { get; set; } = null!;

    [Required]
    [Roll]
    public string Obese { get; set; } = null!;
  }
}
