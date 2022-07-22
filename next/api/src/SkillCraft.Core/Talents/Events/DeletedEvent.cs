namespace SkillCraft.Core.Talents.Events
{
  public class DeletedEvent : DeletedEventBase
  {
    public DeletedEvent(Guid userId) : base(userId)
    {
    }
  }
}
