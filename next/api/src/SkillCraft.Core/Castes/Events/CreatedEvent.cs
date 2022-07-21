using SkillCraft.Core.Castes.Payload;

namespace SkillCraft.Core.Castes.Events
{
  public class CreatedEvent : CreatedEventBase
  {
    public CreatedEvent(CreateCastePayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateCastePayload Payload { get; private set; }
  }
}
