using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Languages.Payloads
{
  public abstract class SaveLanguagePayload
  {
    public string? Description { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    public bool Exotic { get; set; }

    [StringLength(100)]
    public string? Script { get; set; }

    [StringLength(100)]
    public string? TypicalSpeakers { get; set; }

  }
}
