using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Characters
{
  public class Character : Aggregate
  {
    public Character(Guid userId, World world) : base(userId)
    {
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Character() : base()
    {
    }

    public string? Description { get; set; }
    public string Name { get; set; } = null!;
    public string? Player { get; set; }
    public int WorldId { get; set; }

    public double Stature { get; set; }
    public double Weight { get; set; }
    public int Age { get; set; }

    public int Experience { get; set; }
    public int Vitality { get; set; }
    public int Stamina { get; set; }

    public int BloodAlcoholContent { get; set; }
    public int Intoxication { get; set; }

    public World? World { get; set; }

    public override string ToString() => $"{Name} | {base.ToString()}";

    /* TODO(fpion):
     * Race
     * Caste
     * Éducation
     * Nature
     * Attributs
     * Statistiques
     * Dons
     * Handicaps
     * Langues
     * Compétences
     * Combat
     * Talents
     * Pouvoirs
     * Inventaire
     * Notes
     */
  }
}
