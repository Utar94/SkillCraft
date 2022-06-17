using Logitar.Validation;
using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Races.Payloads
{
  public class AttributeBonusPayload
  {
    [Enum(typeof(Attribute))]
    public Attribute Attribute { get; set; }

    [Range(1, 10)]
    public int Bonus { get; set; }
  }
}
