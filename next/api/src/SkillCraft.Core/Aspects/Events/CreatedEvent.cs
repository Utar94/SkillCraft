using SkillCraft.Core.Aspects.Payload;

namespace SkillCraft.Core.Aspects.Events
{
  public class CreatedEvent : CreatedEventBase
  {
    public CreatedEvent(CreateAspectPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateAspectPayload Payload { get; private set; }
  }
}
