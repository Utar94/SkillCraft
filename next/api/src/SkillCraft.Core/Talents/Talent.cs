using Logitar;
using SkillCraft.Core.Talents.Events;
using SkillCraft.Core.Talents.Payload;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Talents
{
  public class Talent : Aggregate
  {
    public Talent(CreateTalentPayload payload, Guid userId, World world, Talent? requiredTalent = null)
    {
      ApplyChange(new CreatedEvent(payload, userId));

      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldSid = world.Sid;

      RequiredTalent = requiredTalent;
      RequiredTalentSid = requiredTalent?.Sid;
    }
    private Talent()
    {
    }

    public World? World { get; private set; }
    public int WorldSid { get; private set; }

    public Talent? RequiredTalent { get; private set; }
    public int? RequiredTalentSid { get; private set; }
    public List<Talent> RequiringTalents { get; private set; } = new();

    public int Tier { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public bool MultipleAcquisition { get; private set; }
    public Skill? Skill { get; private set; }

    public List<TalentOption> Options { get; private set; } = new();

    public void Delete(Guid userId) => ApplyChange(new DeletedEvent(userId));
    public void Update(UpdateTalentPayload payload, Guid userId, Talent? requiredTalent = null)
    {
      ApplyChange(new UpdatedEvent(payload, userId));

      RequiredTalent = requiredTalent;
      RequiredTalentSid = requiredTalent?.Sid;
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

    private void Apply(SaveTalentPayload payload)
    {
      Name = payload.Name.Trim();
      Description = payload.Description?.CleanTrim();

      MultipleAcquisition = payload.MultipleAcquisition;
      Skill = payload.Skill;
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
