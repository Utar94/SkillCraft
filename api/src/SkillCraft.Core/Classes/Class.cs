using SkillCraft.Core.Talents;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Classes
{
  public class Class : EntityBase
  {
    public Class(int tier, Guid userId, World world) : base(userId)
    {
      Tier = tier;
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Class() : base()
    {
    }

    public World? World { get; set; }
    public int WorldId { get; set; }

    public Talent? UniqueTalent { get; set; }
    public int UniqueTalentId { get; set; }

    public int Tier { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public string? OtherRequirements { get; set; }
    public string? OtherTalentsText { get; set; }

    public ICollection<ClassTalent> Talents { get; set; } = new List<ClassTalent>();

    public override string ToString() => $"{Name} | {base.ToString()}";

    public void Validate()
    {
      if (Talents.Count(x => x.Mandatory) > (Tier + 4))
      {
        throw new MandatoryTalentsExceededException(this);
      }
    }
  }
}
