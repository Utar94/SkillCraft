using SkillCraft.Core.Models;

namespace SkillCraft.Core.Characters.Models
{
  public class CharacterModel : AggregateModel
  {
    public string? Description { get; set; }
    public string Name { get; set; } = null!;
    public string? Player { get; set; }

    public double Stature { get; set; }
    public double Weight { get; set; }
    public int Age { get; set; }

    public int Experience { get; set; }
    public int Vitality { get; set; }
    public int Stamina { get; set; }

    public int BloodAlcoholContent { get; set; }
    public int Intoxication { get; set; }
  }
}
