using SkillCraft.Core.Educations.Payload;

namespace SkillCraft.Core.Educations.Events
{
  public class UpdatedEvent : UpdatedEventBase
  {
    public UpdatedEvent(UpdateEducationPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public UpdateEducationPayload Payload { get; private set; }
  }
}
