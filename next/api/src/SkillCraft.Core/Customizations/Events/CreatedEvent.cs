using SkillCraft.Core.Customizations.Payload;

namespace SkillCraft.Core.Customizations.Events
{
  public class CreatedEvent : CreatedEventBase
  {
    public CreatedEvent(CreateCustomizationPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateCustomizationPayload Payload { get; private set; }
  }
}
