using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Characters.Payloads
{
  public abstract class SaveCharacterPayload
  {
    public string? Description { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string? Player { get; set; }

    [Range(0, 999.99)]
    public double Stature { get; set; }

    [Range(0, 999.9)]
    public double Weight { get; set; }

    [Range(0, 9999)]
    public int Age { get; set; }

    [Range(0, 999999)]
    public int Experience { get; set; }

    [Range(-999, 999)]
    public int Vitality { get; set; }

    [Range(-999, 999)]
    public int Stamina { get; set; }

    [Range(0, 99)]
    public int BloodAlcoholContent { get; set; }

    [Range(0, 99)]
    public int Intoxication { get; set; }
  }
}
