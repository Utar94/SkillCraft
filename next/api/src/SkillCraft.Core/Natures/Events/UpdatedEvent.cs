using SkillCraft.Core.Natures.Payload;

namespace SkillCraft.Core.Natures.Events
{
  public class UpdatedEvent : UpdatedEventBase
  {
    public UpdatedEvent(UpdateNaturePayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public UpdateNaturePayload Payload { get; private set; }
  }
}
