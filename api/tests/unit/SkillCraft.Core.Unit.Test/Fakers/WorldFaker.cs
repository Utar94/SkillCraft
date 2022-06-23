using Bogus;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Fakers
{
  public class WorldFaker
  {
    private readonly Faker _faker = new();

    public World Generate()
    {
      return new World(alias: "forgotten-realms", userId: Guid.NewGuid())
      {
        Id = _faker.Random.Number(),
        Name = "Forgotten Realms"
      };
    }
  }
}
