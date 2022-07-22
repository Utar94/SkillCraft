using SkillCraft.Core.Powers.Payload;

namespace SkillCraft.Core.Powers.Events
{
  public class CreatedEvent : CreatedEventBase
  {
    public CreatedEvent(CreatePowerPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreatePowerPayload Payload { get; private set; }
  }
}
