using SkillCraft.Core.Fakers;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Characters
{
  [Trait(Traits.Category, Categories.Unit)]
  public class CharacterLevelUpTests
  {
    private static readonly WorldFaker _worldFaker = new();

    private readonly World _world = _worldFaker.Generate();

    private Guid UserId => _world.CreatedById;

    [Theory]
    [InlineData(Attribute.Intellect)]
    public void Given_Attribute_When_ctor_Then_LevelUp(Attribute attribute)
    {
      var levelUp = new CharacterLevelUp(attribute);
      Assert.Equal(attribute, levelUp.Attribute);
    }

    [Fact]
    public void Given_NullStatistics_When_CalculateStatistics_Then_ArgumentNullException()
    {
      Assert.Equal("statistics", Assert.Throws<ArgumentNullException>(
        () => new CharacterLevelUp(Attribute.Agility).CalculateStatistics(null!)
      ).ParamName);
    }

    [Fact]
    public void Given_Statistics_When_CalculateStatistics_Then_CorrectStatistics()
    {
      var character = new Character(UserId, _world)
      {
        Creation = new()
        {
          BestAttribute = Attribute.Vigor,
          WorstAttribute = Attribute.Intellect,
          MandatoryAttribute1 = Attribute.Mind,
          MandatoryAttribute2 = Attribute.Sensitivity,
          OptionalAttribute1 = Attribute.Coordination,
          OptionalAttribute2 = Attribute.Intellect
        }
      };
      character.Creation.AttributeBases[Attribute.Agility] = 7;
      character.Creation.AttributeBases[Attribute.Coordination] = 7;
      character.Creation.AttributeBases[Attribute.Intellect] = 8;
      character.Creation.AttributeBases[Attribute.Mind] = 10;
      character.Creation.AttributeBases[Attribute.Presence] = 7;
      character.Creation.AttributeBases[Attribute.Sensitivity] = 8;
      character.Creation.AttributeBases[Attribute.Vigor] = 10;

      character.Experience = 11908;
      character.LevelUp(Attribute.Vigor);
      character.LevelUp(Attribute.Presence);
      character.LevelUp(Attribute.Coordination);
      character.LevelUp(Attribute.Vigor);
      character.LevelUp(Attribute.Mind);
      character.LevelUp(Attribute.Mind);

      character.Race = new(UserId, _world)
      {
        ExtraAttributes = 1,
        Name = "Demi-Elfe"
      };
      character.Race.Attributes.Add(Attribute.Coordination, 1);

      character.Nature = new(UserId, _world)
      {
        Attribute = Attribute.Mind,
        Name = "Mystérieux"
      };

      character.Bonuses.Add(new AttributeBonus(Attribute.Mind)
      {
        Description = "Demi-Elfe",
        Id = Guid.NewGuid(),
        Permanent = true,
        Value = 1
      });

      var levelUp = new CharacterLevelUp(Attribute.Vigor);
      character.LevelUps.Add(character.Level, levelUp);

      levelUp.CalculateStatistics(character.Statistics);

      Assert.Equal(8, levelUp.Statistics[Statistic.Constitution]);
      Assert.Equal(0.25, levelUp.Statistics[Statistic.Initiative]);
      Assert.Equal(2, levelUp.Statistics[Statistic.Learning]);
      Assert.Equal(0.8, levelUp.Statistics[Statistic.Power]);
      Assert.Equal(0.5, levelUp.Statistics[Statistic.Precision]);
      Assert.Equal(0.4, levelUp.Statistics[Statistic.Repute]);
      Assert.Equal(0.3, levelUp.Statistics[Statistic.Strength]);
    }
  }
}
