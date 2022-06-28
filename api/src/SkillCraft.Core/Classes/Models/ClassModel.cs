using SkillCraft.Core.Models;
using SkillCraft.Core.Talents.Models;

namespace SkillCraft.Core.Classes.Models
{
  public class ClassModel : EntityBaseModel
  {
    public TalentModel? UniqueTalent { get; set; }

    public int Tier { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public string? OtherRequirements { get; set; }
    public string? OtherTalentsText { get; set; }

    public IEnumerable<ClassTalentModel> Talents { get; set; } = null!;
  }
}
