using SkillCraft.Core.Talents.Models;

namespace SkillCraft.Core.Characters.Models
{
  public class CharacterTalentModel
  {
    public Guid Id { get; set; }

    public TalentModel? Talent { get; set; }
    public TalentOptionModel? Option { get; set; }

    public int Cost { get; set; }
    public string? Description { get; set; }
  }
}
