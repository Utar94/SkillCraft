using SkillCraft.Core.Aspects.Payload;

namespace SkillCraft.Core.Aspects.Events
{
  public class UpdatedEvent : UpdatedEventBase
  {
    public UpdatedEvent(UpdateAspectPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public UpdateAspectPayload Payload { get; private set; }
  }
}
