namespace SkillCraft.Core
{
  public abstract class Event
  {
    protected Event(Guid userId)
    {
      OccurredAt = DateTime.UtcNow;
      UserId = userId;
    }

    public DateTime OccurredAt { get; set; } // Empty setter for deserialization
    public Guid UserId { get; private set; }
  }

  public abstract class CreatedEventBase : Event
  {
    protected CreatedEventBase(Guid userId) : base(userId)
    {
    }
  }

  public abstract class DeletedEventBase : Event
  {
    protected DeletedEventBase(Guid userId) : base(userId)
    {
    }
  }

  public abstract class UpdatedEventBase : Event
  {
    protected UpdatedEventBase(Guid userId) : base(userId)
    {
    }
  }
}
