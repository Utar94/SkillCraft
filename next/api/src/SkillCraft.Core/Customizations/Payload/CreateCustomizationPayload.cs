namespace SkillCraft.Core.Customizations.Payload
{
  public class CreateCustomizationPayload : SaveCustomizationPayload
  {
    public CustomizationType Type { get; set; }
  }
}
