using SkillCraft.Core.Talents.Payload;

namespace SkillCraft.Core.Talents.Events
{
  public class UpdatedEvent : UpdatedEventBase
  {
    public UpdatedEvent(UpdateTalentPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public UpdateTalentPayload Payload { get; private set; }
  }
}
