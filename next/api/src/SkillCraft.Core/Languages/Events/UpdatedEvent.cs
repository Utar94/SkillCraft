using SkillCraft.Core.Languages.Payload;

namespace SkillCraft.Core.Languages.Events
{
  public class UpdatedEvent : UpdatedEventBase
  {
    public UpdatedEvent(UpdateLanguagePayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public UpdateLanguagePayload Payload { get; private set; }
  }
}
