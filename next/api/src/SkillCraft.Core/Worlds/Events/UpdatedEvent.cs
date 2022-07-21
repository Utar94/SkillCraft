using SkillCraft.Core.Worlds.Payload;

namespace SkillCraft.Core.Worlds.Events
{
  public class UpdatedEvent : UpdatedEventBase
  {
    public UpdatedEvent(UpdateWorldPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public UpdateWorldPayload Payload { get; private set; }
  }
}
