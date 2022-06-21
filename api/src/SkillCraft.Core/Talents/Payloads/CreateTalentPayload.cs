using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Talents.Payloads
{
  public class CreateTalentPayload : SaveTalentPayload
  {
    [Range(0, 3)]
    public int Tier { get; set; }
  }
}
