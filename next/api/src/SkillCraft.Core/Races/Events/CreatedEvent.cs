using SkillCraft.Core.Races.Payload;

namespace SkillCraft.Core.Races.Events
{
  public class CreatedEvent : CreatedEventBase
  {
    public CreatedEvent(CreateRacePayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateRacePayload Payload { get; private set; }
  }
}
