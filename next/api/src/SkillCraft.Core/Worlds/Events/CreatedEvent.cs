using SkillCraft.Core.Worlds.Payload;

namespace SkillCraft.Core.Worlds.Events
{
  public class CreatedEvent : CreatedEventBase
  {
    public CreatedEvent(CreateWorldPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateWorldPayload Payload { get; private set; }
  }
}
