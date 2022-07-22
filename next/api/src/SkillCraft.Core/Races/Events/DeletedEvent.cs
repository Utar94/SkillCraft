namespace SkillCraft.Core.Races.Events
{
  public class DeletedEvent : DeletedEventBase
  {
    public DeletedEvent(Guid userId) : base(userId)
    {
    }
  }
}
