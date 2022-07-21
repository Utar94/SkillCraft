using SkillCraft.Core.Castes.Payload;

namespace SkillCraft.Core.Castes.Events
{
  public class UpdatedEvent : UpdatedEventBase
  {
    public UpdatedEvent(UpdateCastePayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public UpdateCastePayload Payload { get; private set; }
  }
}
