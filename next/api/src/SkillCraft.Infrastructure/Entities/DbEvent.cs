using SkillCraft.Core;
using System.Text.Json;

namespace SkillCraft.Infrastructure.Entities
{
  internal class DbEvent
  {
    private DbEvent()
    {
    }

    public Guid Id { get; private set; }
    public long Sid { get; private set; }

    public DateTime OccurredAt { get; private set; }
    public Guid UserId { get; private set; }

    public string EventType { get; private set; } = null!;
    public string EventData { get; private set; } = null!;

    public string AggregateType { get; private set; } = null!;
    public Guid AggregateId { get; private set; }

    public static IEnumerable<DbEvent> FromChanges(Aggregate aggregate)
    {
      var aggregateType = aggregate?.GetType() ?? throw new ArgumentNullException(nameof(aggregate));

      return aggregate.Changes.Select(change =>
      {
        var eventType = change.GetType();

        return new DbEvent
        {
          OccurredAt = change.OccurredAt,
          UserId = change.UserId,
          EventType = eventType.GetName(),
          EventData = JsonSerializer.Serialize(change, eventType),
          AggregateType = aggregateType.GetName(),
          AggregateId = aggregate.Id
        };
      });
    }
  }
}
