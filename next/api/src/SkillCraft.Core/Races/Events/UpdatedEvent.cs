using SkillCraft.Core.Races.Payload;

namespace SkillCraft.Core.Races.Events
{
  public class UpdatedEvent : UpdatedEventBase
  {
    public UpdatedEvent(UpdateRacePayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public UpdateRacePayload Payload { get; private set; }
  }
}
