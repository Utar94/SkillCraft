using Logitar;
using SkillCraft.Core.Customizations.Events;
using SkillCraft.Core.Customizations.Payload;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Customizations
{
  public class Customization : Aggregate
  {
    public Customization(CreateCustomizationPayload payload, Guid userId, World world)
    {
      ApplyChange(new CreatedEvent(payload, userId));

      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldSid = world.Sid;
    }
    private Customization()
    {
    }

    public World? World { get; private set; }
    public int WorldSid { get; private set; }

    public CustomizationType Type { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public void Delete(Guid userId) => ApplyChange(new DeletedEvent(userId));
    public void Update(UpdateCustomizationPayload payload, Guid userId) => ApplyChange(new UpdatedEvent(payload, userId));

    protected virtual void Apply(CreatedEvent @event)
    {
      Type = @event.Payload.Type;

      Apply(@event.Payload);
    }
    protected virtual void Apply(DeletedEvent @event)
    {
    }
    protected virtual void Apply(UpdatedEvent @event)
    {
      Apply(@event.Payload);
    }

    private void Apply(SaveCustomizationPayload payload)
    {
      Name = payload.Name.Trim();
      Description = payload.Description?.CleanTrim();
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
