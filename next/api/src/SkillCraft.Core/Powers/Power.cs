using Logitar;
using SkillCraft.Core.Powers.Events;
using SkillCraft.Core.Powers.Payload;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Powers
{
  public class Power : Aggregate
  {
    public Power(CreatePowerPayload payload, Guid userId, World world)
    {
      ApplyChange(new CreatedEvent(payload, userId));

      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldSid = world.Sid;
    }
    private Power()
    {
    }

    public World? World { get; private set; }
    public int WorldSid { get; private set; }

    public int Tier { get; private set; }
    public string Name { get; private set; } = null!;
    public string[]? Descriptions { get; private set; }

    public IncantationType Incantation { get; private set; }
    public bool IsRitual { get; private set; }

    public bool IsSomatic { get; private set; }
    public bool IsVerbal { get; private set; }

    public int? Duration { get; private set; }
    public bool IsConcentration { get; private set; }

    public int? Range { get; private set; }

    public string? Ingredients { get; private set; }
    public bool IsFocus { get; private set; }

    public void Delete(Guid userId) => ApplyChange(new DeletedEvent(userId));
    public void Update(UpdatePowerPayload payload, Guid userId)
    {
      ApplyChange(new UpdatedEvent(payload, userId));
    }

    protected virtual void Apply(CreatedEvent @event)
    {
      Tier = @event.Payload.Tier;

      Apply(@event.Payload);
    }
    protected virtual void Apply(DeletedEvent @event)
    {
    }
    protected virtual void Apply(UpdatedEvent @event)
    {
      Apply(@event.Payload);
    }

    private void Apply(SavePowerPayload payload)
    {
      Name = payload.Name.Trim();
      Descriptions = payload.Descriptions == null ? null : new[]
      {
        payload.Descriptions.Global!,
        payload.Descriptions.FirstLevel,
        payload.Descriptions.SecondLevel,
        payload.Descriptions.ThirdLevel
      }.Where(x => x != null).ToArray();

      Incantation = payload.Incantation;
      IsRitual = payload.IsRitual;

      IsSomatic = payload.IsSomatic;
      IsVerbal = payload.IsVerbal;

      Duration = payload.Duration;
      IsConcentration = payload.IsConcentration;

      Range = payload.Range;

      Ingredients = payload.Ingredients?.CleanTrim();
      IsFocus = payload.IsFocus;
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
