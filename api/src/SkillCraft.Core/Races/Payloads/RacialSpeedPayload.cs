using Logitar.Validation;
using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Races.Payloads
{
  public class RacialSpeedPayload
  {
    [Enum(typeof(SpeedType))]
    public SpeedType Type { get; set; }

    [Range(1, 99)]
    public int Value { get; set; }
  }
}
