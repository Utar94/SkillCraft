using Logitar.Validation;

namespace SkillCraft.Core.Customizations.Payloads
{
  public class CreateCustomizationPayload : SaveCustomizationPayload
  {
    [Enum(typeof(CustomizationType))]
    public CustomizationType Type { get; set; }
  }
}
