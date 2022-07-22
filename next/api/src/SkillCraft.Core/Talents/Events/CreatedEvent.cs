using SkillCraft.Core.Talents.Payload;

namespace SkillCraft.Core.Talents.Events
{
  public class CreatedEvent : CreatedEventBase
  {
    public CreatedEvent(CreateTalentPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateTalentPayload Payload { get; private set; }
  }
}
