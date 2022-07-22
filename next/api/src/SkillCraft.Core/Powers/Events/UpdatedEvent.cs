using SkillCraft.Core.Powers.Payload;

namespace SkillCraft.Core.Powers.Events
{
  public class UpdatedEvent : UpdatedEventBase
  {
    public UpdatedEvent(UpdatePowerPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public UpdatePowerPayload Payload { get; private set; }
  }
}
