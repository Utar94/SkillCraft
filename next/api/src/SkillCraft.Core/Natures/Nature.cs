using Logitar;
using SkillCraft.Core.Customizations;
using SkillCraft.Core.Natures.Events;
using SkillCraft.Core.Natures.Payload;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Natures
{
  public class Nature : Aggregate
  {
    public Nature(CreateNaturePayload payload, Guid userId, World world, Customization? feat = null)
    {
      ApplyChange(new CreatedEvent(payload, userId));

      Feat = feat;
      FeatSid = feat?.Sid;
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldSid = world.Sid;
    }
    private Nature()
    {
    }

    public World? World { get; private set; }
    public int WorldSid { get; private set; }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public Attribute Attribute { get; private set; }
    public Customization? Feat { get; private set; }
    public int? FeatSid { get; private set; }

    public void Delete(Guid userId) => ApplyChange(new DeletedEvent(userId));
    public void Update(UpdateNaturePayload payload, Guid userId, Customization? feat = null)
    {
      ApplyChange(new UpdatedEvent(payload, userId));

      Feat = feat;
      FeatSid = feat?.Sid;
    }

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

    private void Apply(SaveNaturePayload payload)
    {
      Name = payload.Name.Trim();
      Description = payload.Description?.CleanTrim();

      Attribute = payload.Attribute;
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
