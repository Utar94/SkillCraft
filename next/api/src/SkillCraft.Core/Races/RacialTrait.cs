using Logitar;

namespace SkillCraft.Core.Races
{
  public class RacialTrait
  {
    public RacialTrait(Race race, string name, string? description = null)
    {
      Id = Guid.NewGuid();

      Race = race ?? throw new ArgumentNullException(nameof(race));
      RaceSid = race.Sid;

      Update(name, description);
    }
    private RacialTrait()
    {
    }

    public Guid Id { get; private set; }
    public int Sid { get; private set; }

    public Race? Race { get; private set; }
    public int RaceSid { get; private set; }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public void Update(string name, string? description)
    {
      Name = name?.Trim() ?? throw new ArgumentNullException(nameof(name));
      Description = description?.CleanTrim();
    }
  }
}
