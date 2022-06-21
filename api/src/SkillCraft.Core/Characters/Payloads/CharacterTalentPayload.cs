using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Characters.Payloads
{
  public class CharacterTalentPayload
  {
    public Guid TalentId { get; set; }

    [Range(0, 5)]
    public int Cost { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }
  }
}
