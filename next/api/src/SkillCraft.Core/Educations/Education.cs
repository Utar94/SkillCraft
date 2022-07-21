using Logitar;
using SkillCraft.Core.Educations.Events;
using SkillCraft.Core.Educations.Payload;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Educations
{
  public class Education : Aggregate
  {
    public Education(CreateEducationPayload payload, Guid userId, World world)
    {
      ApplyChange(new CreatedEvent(payload, userId));

      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldSid = world.Sid;
    }
    private Education()
    {
    }

    public World? World { get; private set; }
    public int WorldSid { get; private set; }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public Skill Skill { get; private set; }
    public int WealthMultiplier { get; private set; }

    public void Delete(Guid userId) => ApplyChange(new DeletedEvent(userId));
    public void Update(UpdateEducationPayload payload, Guid userId) => ApplyChange(new UpdatedEvent(payload, userId));

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

    private void Apply(SaveEducationPayload payload)
    {
      Name = payload.Name.Trim();
      Description = payload.Description?.CleanTrim();

      Skill = payload.Skill;
      WealthMultiplier = payload.WealthMultiplier;
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
