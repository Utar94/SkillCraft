using SkillCraft.Core.Natures.Payload;

namespace SkillCraft.Core.Natures.Events
{
  public class CreatedEvent : CreatedEventBase
  {
    public CreatedEvent(CreateNaturePayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateNaturePayload Payload { get; private set; }
  }
}
