using Logitar;
using SkillCraft.Core.Aspects;
using SkillCraft.Core.Castes;
using SkillCraft.Core.Customizations;
using SkillCraft.Core.Educations;
using SkillCraft.Core.Languages;
using SkillCraft.Core.Natures;
using SkillCraft.Core.Races;
using SkillCraft.Core.Worlds.Events;
using SkillCraft.Core.Worlds.Payload;

namespace SkillCraft.Core.Worlds
{
  public class World : Aggregate
  {
    public World(CreateWorldPayload payload, Guid userId)
    {
      ApplyChange(new CreatedEvent(payload, userId));
    }
    private World()
    {
    }

    public string Alias { get; private set; } = null!;
    public string AliasNormalized
    {
      get => Alias.ToUpper();
      set { /* Empty setter for EntityFrameworkCore */ }
    }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public List<Aspect> Aspects { get; private set; } = new();
    public List<Caste> Castes { get; private set; } = new();
    public List<Customization> Customizations { get; private set; } = new();
    public List<Education> Educations { get; private set; } = new();
    public List<Language> Languages { get; private set; } = new();
    public List<Nature> Natures { get; private set; } = new();
    public List<Race> Races { get; private set; } = new();

    public void Delete(Guid userId) => ApplyChange(new DeletedEvent(userId));
    public void Update(UpdateWorldPayload payload, Guid userId) => ApplyChange(new UpdatedEvent(payload, userId));

    protected virtual void Apply(CreatedEvent @event)
    {
      Alias = @event.Payload.Alias;

      Apply(@event.Payload);
    }
    protected virtual void Apply(DeletedEvent @event)
    {
    }
    protected virtual void Apply(UpdatedEvent @event)
    {
      Apply(@event.Payload);
    }

    private void Apply(SaveWorldPayload payload)
    {
      Name = payload.Name.Trim();
      Description = payload.Description?.CleanTrim();
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
