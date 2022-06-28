using SkillCraft.Core.Powers.Models;

namespace SkillCraft.Core.Characters.Models
{
  public class CharacterPowerModel
  {
    public PowerModel? Power { get; set; }

    public int Cost { get; set; }
    public string? Description { get; set; }
  }
}
