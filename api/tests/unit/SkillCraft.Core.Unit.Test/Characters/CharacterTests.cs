using SkillCraft.Core.Fakers;
using SkillCraft.Core.Talents;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Characters
{
  [Trait(Traits.Category, Categories.Unit)]
  public class CharacterTests
  {
    private static readonly WorldFaker _worldFaker = new();

    private readonly ExperienceTable _experienceTable = new();
    private readonly World _world = _worldFaker.Generate();
    private readonly Character _character;

    public CharacterTests()
    {
      _character = new(UserId, _world);
    }

    private Guid UserId => _world.CreatedById;

    [Fact]
    public void Given_Character_When_getMaxStamina_Then_CorrectMaxStamina()
    {
      _character.Creation = new();
      _character.Creation.AttributeBases[Attribute.Vigor] = 10;
      _character.Creation.BestAttribute = Attribute.Vigor;

      _character.Experience = 11908;
      _character.LevelUp(Attribute.Vigor);
      _character.LevelUp(Attribute.Presence);
      _character.LevelUp(Attribute.Coordination);
      _character.LevelUp(Attribute.Vigor);
      _character.LevelUp(Attribute.Mind);
      _character.LevelUp(Attribute.Mind);

      _character.Bonuses.Add(new OtherBonus(OtherBonusTarget.Stamina)
      {
        Description = "Endurance I",
        Permanent = true,
        Value = 5
      });
      _character.Bonuses.Add(new OtherBonus(OtherBonusTarget.Stamina)
      {
        Description = "Endurance II",
        Permanent = true,
        Value = 10
      });

      Assert.Equal(92, _character.MaxStamina);
    }

    [Fact]
    public void Given_Character_When_getMaxVitality_Then_CorrectMaxVitality()
    {
      _character.Creation = new();
      _character.Creation.AttributeBases[Attribute.Vigor] = 10;
      _character.Creation.BestAttribute = Attribute.Vigor;

      _character.Experience = 11908;
      _character.LevelUp(Attribute.Vigor);
      _character.LevelUp(Attribute.Presence);
      _character.LevelUp(Attribute.Coordination);
      _character.LevelUp(Attribute.Vigor);
      _character.LevelUp(Attribute.Mind);
      _character.LevelUp(Attribute.Mind);

      Assert.Equal(77, _character.MaxVitality);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(42)]
    [InlineData(123)]
    [InlineData(1099)]
    [InlineData(1100)]
    [InlineData(268000)]
    [InlineData(999999)]
    public void Given_Experience_When_getLevel_Then_CorrectLevel(int experience)
    {
      int expected = _experienceTable.GetLevel(experience);

      _character.Experience = experience;

      Assert.Equal(expected, _character.Level);
    }

    [Theory]
    [InlineData(9)]
    public void Given_Level_When_getTotalLearningPoints_Then_CorrectTotalLearningPoints(int level)
    {
      _character.Creation = new();
      _character.Creation.AttributeBases[Attribute.Intellect] = 10;
      _character.Experience = _experienceTable.GetThreshold(level);

      for (int current = 0; current < level; current++)
      {
        _character.LevelUp(Attribute.Mind);
      }

      Assert.Equal(5 + 2 * level, _character.TotalLearningPoints);
    }

    [Theory]
    [InlineData(9)]
    public void Given_Level_When_getTotalTalentPoints_Then_CorrectTotalTalentPoints(int level)
    {
      _character.Experience = _experienceTable.GetThreshold(level);

      int expected = (level + 1) * 4;

      Assert.Equal(expected, _character.TotalTalentPoints);
    }

    [Theory]
    [InlineData(Attribute.Vigor)]
    public void Given_LevelUps_When_LevelUp_Then_LeveledUp(Attribute attribute)
    {
      _character.Experience = 11908;

      _character.LevelUp(Attribute.Vigor);
      _character.LevelUp(Attribute.Presence);
      _character.LevelUp(Attribute.Coordination);
      _character.LevelUp(Attribute.Vigor);
      _character.LevelUp(Attribute.Mind);
      _character.LevelUp(Attribute.Mind);

      CharacterLevelUp levelUp = _character.LevelUp(attribute);
      Assert.Same(_character.LevelUps[_character.Level], levelUp);
    }

    [Theory]
    [InlineData(Attribute.Vigor)]
    public void Given_MaxLevel_When_LevelUp_Then_InvalidOperationException(Attribute attribute)
    {
      _character.Experience = 11908;

      _character.LevelUp(Attribute.Vigor);
      _character.LevelUp(Attribute.Presence);
      _character.LevelUp(Attribute.Coordination);
      _character.LevelUp(Attribute.Vigor);
      _character.LevelUp(Attribute.Mind);
      _character.LevelUp(Attribute.Mind);
      _character.LevelUp(attribute);

      Assert.Throws<InvalidOperationException>(() => _character.LevelUp(attribute));
    }

    [Theory]
    [InlineData(Attribute.Sensitivity)]
    public void Given_NoLevelUp_When_LevelUp_Then_LeveledUp(Attribute attribute)
    {
      _character.Experience = _experienceTable.GetThreshold(1);

      CharacterLevelUp levelUp = _character.LevelUp(attribute);
      Assert.Same(_character.LevelUps.Values.Single(), levelUp);
    }

    [Fact]
    public void Given_SkillRanks_When_Validate_Then_Success()
    {
      _character.SkillRanks.Add(new SkillRank(Skill.Acrobatics, training: false));
      _character.SkillRanks.Add(new SkillRank(Skill.Acrobatics, training: false));
      _character.SkillRanks.Add(new SkillRank(Skill.Stealth, training: true));

      _character.Validate();
    }

    [Fact]
    public void Given_SkillRanks_When_getRemainingLearningPoints_Then_CorrectRemainingLearningPoints()
    {
      _character.SkillRanks.Add(new SkillRank(Skill.Craft, training: false));
      _character.SkillRanks.Add(new SkillRank(Skill.Orientation, training: true));

      Assert.Equal(2, _character.RemainingLearningPoints);
    }

    [Fact]
    public void Given_SkillRanks_When_getSpentLearningPoints_Then_CorrectSpentLearningPoints()
    {
      _character.SkillRanks.Add(new SkillRank(Skill.Acrobatics, false));
      _character.SkillRanks.Add(new SkillRank(Skill.Insight, true));
      _character.SkillRanks.Add(new SkillRank(Skill.Sorcery, true));

      int expected = _character.SkillRanks.Sum(x => x.Cost);

      Assert.Equal(expected, _character.SpentLearningPoints);
    }

    [Fact]
    public void Given_SpentLearningPointsExceeded_When_Validate_Then_SpentLearningPointsExceededException()
    {
      _character.SkillRanks.Add(new(Skill.Insight, training: false));
      _character.SkillRanks.Add(new(Skill.Insight, training: false));
      _character.SkillRanks.Add(new(Skill.Insight, training: false));

      var exception = Assert.Throws<SpentLearningPointsExceededException>(() => _character.Validate());
      Assert.Same(_character, exception.Character);
    }

    [Fact]
    public void Given_SpentTalentPointsExceeded_When_Validate_Then_SpentTalentPointsExceededException()
    {
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 3, UserId, _world))
      {
        Cost = 5
      });

      var exception = Assert.Throws<SpentTalentPointsExceededException>(() => _character.Validate());
      Assert.Same(_character, exception.Character);
    }

    [Fact]
    public void Given_Talents_When_getRemainingTalentPoints_Then_CorrectRemainingTalentPoints()
    {
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, UserId, _world))
      {
        Cost = 0
      });
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, UserId, _world))
      {
        Cost = 1
      });
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, UserId, _world))
      {
        Cost = 2
      });

      Assert.Equal(1, _character.RemainingTalentPoints);
    }

    [Fact]
    public void Given_Talents_When_getSpentTalentPoints_Then_CorrectSpentTalentPoints()
    {
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, UserId, _world))
      {
        Cost = 0
      });
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, UserId, _world))
      {
        Cost = 1
      });
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, UserId, _world))
      {
        Cost = 2
      });
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 1, UserId, _world))
      {
        Cost = 3
      });

      Assert.Equal(6, _character.SpentTalentPoints);
    }

    [Fact]
    public void Given_Talents_When_Validate_Then_Success()
    {
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, UserId, _world))
      {
        Cost = 0
      });
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, UserId, _world))
      {
        Cost = 1
      });
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, UserId, _world))
      {
        Cost = 2
      });

      _character.Validate();
    }
  }
}
