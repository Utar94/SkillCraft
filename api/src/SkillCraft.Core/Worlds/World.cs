using SkillCraft.Core.Aspects;
using SkillCraft.Core.Castes;
using SkillCraft.Core.Disabilities;
using SkillCraft.Core.Educations;
using SkillCraft.Core.Gifts;
using SkillCraft.Core.Languages;
using SkillCraft.Core.Natures;

namespace SkillCraft.Core.Worlds
{
  public class World : Aggregate
  {
    public World(string alias, Guid userId) : base(userId)
    {
      Alias = alias?.ToLowerInvariant() ?? throw new ArgumentNullException(nameof(alias));
    }
    private World() : base()
    {
    }

    public string Alias { get; set; } = null!;
    public Confidentiality Confidentiality { get; set; }
    public string? Description { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Aspect> Aspects { get; set; } = new List<Aspect>();
    public ICollection<Caste> Castes { get; set; } = new List<Caste>();
    public ICollection<Disability> Disabilities { get; set; } = new List<Disability>();
    public ICollection<Education> Educations { get; set; } = new List<Education>();
    public ICollection<Gift> Gifts { get; set; } = new List<Gift>();
    public ICollection<Language> Languages { get; set; } = new List<Language>();
    public ICollection<Nature> Natures { get; set; } = new List<Nature>();

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
