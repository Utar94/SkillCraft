using Logitar.Validation;

namespace SkillCraft.Core.Characters.Payloads
{
  public class LevelUpCharacterPayload
  {
    [Enum(typeof(Attribute))]
    public Attribute Attribute { get; set; }
  }
}
