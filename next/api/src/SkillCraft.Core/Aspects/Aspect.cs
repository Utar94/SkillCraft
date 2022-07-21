using Logitar;
using SkillCraft.Core.Aspects.Events;
using SkillCraft.Core.Aspects.Payload;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Aspects
{
  public class Aspect : Aggregate
  {
    public Aspect(CreateAspectPayload payload, Guid userId, World world)
    {
      ApplyChange(new CreatedEvent(payload, userId));

      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldSid = world.Sid;
    }
    private Aspect()
    {
    }

    public World? World { get; private set; }
    public int WorldSid { get; private set; }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public Attribute MandatoryAttribute1 { get; private set; }
    public Attribute MandatoryAttribute2 { get; private set; }
    public Attribute OptionalAttribute1 { get; private set; }
    public Attribute OptionalAttribute2 { get; private set; }

    public Skill Skill1 { get; private set; }
    public Skill Skill2 { get; private set; }

    public void Delete(Guid userId) => ApplyChange(new DeletedEvent(userId));
    public void Update(UpdateAspectPayload payload, Guid userId) => ApplyChange(new UpdatedEvent(payload, userId));

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

    private void Apply(SaveAspectPayload payload)
    {
      Name = payload.Name.Trim();
      Description = payload.Description?.CleanTrim();

      MandatoryAttribute1 = payload.MandatoryAttribute1;
      MandatoryAttribute2 = payload.MandatoryAttribute2;
      OptionalAttribute1 = payload.OptionalAttribute1;
      OptionalAttribute2 = payload.OptionalAttribute2;

      Skill1 = payload.Skill1;
      Skill2 = payload.Skill2;
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
