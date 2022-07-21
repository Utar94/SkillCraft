using Logitar;
using SkillCraft.Core.Castes.Events;
using SkillCraft.Core.Castes.Payload;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Castes
{
  public class Caste : Aggregate
  {
    public Caste(CreateCastePayload payload, Guid userId, World world)
    {
      ApplyChange(new CreatedEvent(payload, userId));

      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldSid = world.Sid;
    }
    private Caste()
    {
    }

    public World? World { get; private set; }
    public int WorldSid { get; private set; }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public Skill Skill { get; private set; }
    public string? WealthRoll { get; private set; }

    public void Delete(Guid userId) => ApplyChange(new DeletedEvent(userId));
    public void Update(UpdateCastePayload payload, Guid userId) => ApplyChange(new UpdatedEvent(payload, userId));

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

    private void Apply(SaveCastePayload payload)
    {
      Name = payload.Name.Trim();
      Description = payload.Description?.CleanTrim();

      Skill = payload.Skill;
      WealthRoll = payload.WealthRoll;
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
