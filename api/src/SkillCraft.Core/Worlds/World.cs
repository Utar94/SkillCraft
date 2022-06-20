using Logitar;
using SkillCraft.Core.Aspects;
using SkillCraft.Core.Castes;
using SkillCraft.Core.Conditions;
using SkillCraft.Core.Customizations;
using SkillCraft.Core.Educations;
using SkillCraft.Core.Languages;
using SkillCraft.Core.Natures;
using SkillCraft.Core.Races;

namespace SkillCraft.Core.Worlds
{
  public class World : EntityBase
  {
    public World(string alias, Guid userId) : base(userId)
    {
      Alias = alias?.CleanTrim()?.ToLowerInvariant() ?? throw new ArgumentNullException(nameof(alias));
    }
    private World() : base()
    {
    }

    public string Alias { get; set; } = null!;
    public string? Description { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Aspect> Aspects { get; set; } = new List<Aspect>();
    public ICollection<Caste> Castes { get; set; } = new List<Caste>();
    public ICollection<Condition> Conditions { get; set; } = new List<Condition>();
    public ICollection<Customization> Customizations { get; set; } = new List<Customization>();
    public ICollection<Education> Educations { get; set; } = new List<Education>();
    public ICollection<Language> Languages { get; set; } = new List<Language>();
    public ICollection<Nature> Natures { get; set; } = new List<Nature>();
    public ICollection<Race> Races { get; set; } = new List<Race>();

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
