using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Powers.Payloads
{
  public class CreatePowerPayload : SavePowerPayload
  {
    [Range(0, 3)]
    public int Tier { get; set; }
  }
}
