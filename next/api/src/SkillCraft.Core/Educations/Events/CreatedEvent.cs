using SkillCraft.Core.Educations.Payload;

namespace SkillCraft.Core.Educations.Events
{
  public class CreatedEvent : CreatedEventBase
  {
    public CreatedEvent(CreateEducationPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateEducationPayload Payload { get; private set; }
  }
}
