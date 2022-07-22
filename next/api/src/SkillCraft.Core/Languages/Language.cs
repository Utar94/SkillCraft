using Logitar;
using SkillCraft.Core.Languages.Events;
using SkillCraft.Core.Languages.Payload;
using SkillCraft.Core.Races;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Languages
{
  public class Language : Aggregate
  {
    public Language(CreateLanguagePayload payload, Guid userId, World world)
    {
      ApplyChange(new CreatedEvent(payload, userId));

      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldSid = world.Sid;
    }
    private Language()
    {
    }

    public World? World { get; private set; }
    public int WorldSid { get; private set; }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public bool IsExotic { get; private set; }
    public string? Script { get; private set; }
    public string? TypicalSpeakers { get; private set; }

    public List<Race> Races { get; private set; } = new();

    public void Delete(Guid userId) => ApplyChange(new DeletedEvent(userId));
    public void Update(UpdateLanguagePayload payload, Guid userId) => ApplyChange(new UpdatedEvent(payload, userId));

    protected virtual void Apply(CreatedEvent @event)
    {
      Apply(@event.Payload);
    }
    protected virtual void Apply(DeletedEvent @event)
    {
    }
    protected virtual void Apply(UpdatedEvent @event)
    {
      Apply(@event.Payload);
    }

    private void Apply(SaveLanguagePayload payload)
    {
      Name = payload.Name.Trim();
      Description = payload.Description?.CleanTrim();

      IsExotic = payload.IsExotic;
      Script = payload.Script?.CleanTrim();
      TypicalSpeakers = payload.TypicalSpeakers?.CleanTrim();
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
