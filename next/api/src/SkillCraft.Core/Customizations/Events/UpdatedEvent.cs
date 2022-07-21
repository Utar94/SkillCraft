using SkillCraft.Core.Customizations.Payload;

namespace SkillCraft.Core.Customizations.Events
{
  public class UpdatedEvent : UpdatedEventBase
  {
    public UpdatedEvent(UpdateCustomizationPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public UpdateCustomizationPayload Payload { get; private set; }
  }
}
