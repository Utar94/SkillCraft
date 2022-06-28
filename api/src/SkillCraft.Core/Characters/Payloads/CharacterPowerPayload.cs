using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Characters.Payloads
{
  public class CharacterPowerPayload
  {
    public Guid PowerId { get; set; }

    [Range(0, 5)]
    public int Cost { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }
  }
}
