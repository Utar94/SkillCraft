using SkillCraft.Core.Fakers;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Characters
{
  [Trait(Traits.Category, Categories.Unit)]
  public class CharacterAttributeTests
  {
    private static readonly WorldFaker _worldFaker = new();

    private readonly World _world = _worldFaker.Generate();
    private readonly Character _character;

    public CharacterAttributeTests()
    {
      _character = new(UserId, _world);
    }

    private Guid UserId => _world.CreatedById;

    [Fact]
    public void Given_PopulatedCharacter_Then_CorrectScoreAndModifier()
    {
      _character.Creation = new()
      {
        BestAttribute = Attribute.Vigor,
        WorstAttribute = Attribute.Intellect,
        MandatoryAttribute1 = Attribute.Mind,
        MandatoryAttribute2 = Attribute.Sensitivity,
        OptionalAttribute1 = Attribute.Coordination,
        OptionalAttribute2 = Attribute.Intellect
      };
      _character.Creation.AttributeBases[Attribute.Agility] = 7;
      _character.Creation.AttributeBases[Attribute.Coordination] = 7;
      _character.Creation.AttributeBases[Attribute.Intellect] = 8;
      _character.Creation.AttributeBases[Attribute.Mind] = 10;
      _character.Creation.AttributeBases[Attribute.Presence] = 7;
      _character.Creation.AttributeBases[Attribute.Sensitivity] = 8;
      _character.Creation.AttributeBases[Attribute.Vigor] = 10;
      _character.Experience = 11908;

      _character.Race = new(UserId, _world)
      {
        ExtraAttributes = 1,
        Name = "Demi-Elfe"
      };
      _character.Race.Attributes.Add(Attribute.Coordination, 1);

      _character.Nature = new(UserId, _world)
      {
        Attribute = Attribute.Mind,
        Name = "Mystérieux"
      };

      _character.LevelUp(Attribute.Vigor);
      _character.LevelUp(Attribute.Presence);
      _character.LevelUp(Attribute.Coordination);
      _character.LevelUp(Attribute.Vigor);
      _character.LevelUp(Attribute.Mind);
      _character.LevelUp(Attribute.Mind);

      _character.Bonuses.Add(new AttributeBonus(Attribute.Mind)
      {
        Description = "Demi-Elfe",
        Id = Guid.NewGuid(),
        Permanent = true,
        Value = 1
      });

      var expected = new Dictionary<Attribute, Tuple<int, int>>
      {
        [Attribute.Agility] = new(7, -2),
        [Attribute.Coordination] = new(10, 0),
        [Attribute.Intellect] = new(10, 0),
        [Attribute.Mind] = new(16, 3),
        [Attribute.Presence] = new(8, -1),
        [Attribute.Sensitivity] = new(10, 0),
        [Attribute.Vigor] = new(15, 2)
      };

      foreach (Attribute attribute in Enum.GetValues(typeof(Attribute)))
      {
        var characterAttribute = new CharacterAttribute(attribute, _character);

        Assert.Equal(expected[attribute].Item1, characterAttribute.Score);
        Assert.Equal(expected[attribute].Item2, characterAttribute.Modifier);
      }
    }
  }
}
