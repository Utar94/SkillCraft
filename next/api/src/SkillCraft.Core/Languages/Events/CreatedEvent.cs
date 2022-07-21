using SkillCraft.Core.Languages.Payload;

namespace SkillCraft.Core.Languages.Events
{
  public class CreatedEvent : CreatedEventBase
  {
    public CreatedEvent(CreateLanguagePayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateLanguagePayload Payload { get; private set; }
  }
}
