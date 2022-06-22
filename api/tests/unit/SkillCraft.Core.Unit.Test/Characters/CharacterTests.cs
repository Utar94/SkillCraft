using SkillCraft.Core.Talents;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Characters
{
  [Trait(Traits.Category, Categories.Unit)]
  public class CharacterTests
  {
    private readonly ExperienceTable _experienceTable = new();

    private readonly Guid _userId = Guid.NewGuid();

    private readonly Character _character;
    private readonly World _world;

    public CharacterTests()
    {
      _world = new("alias", _userId);
      _character = new(_userId, _world);
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
    public void Given_Level_When_getTotalTalentPoints_Then_CorrectTotalTalentPoints(int level)
    {
      _character.Experience = _experienceTable.GetThreshold(level);

      int expected = (level + 1 ) * 4;

      Assert.Equal(expected, _character.TotalTalentPoints);
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
    public void Given_SpentTalentPointsExceeded_When_Validate_Then_SpentTalentPointsExceededException()
    {
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 3, _userId, _world))
      {
        Cost = 5
      });

      var exception = Assert.Throws<SpentTalentPointsExceededException>(() => _character.Validate());
      Assert.Same(_character, exception.Character);
    }

    [Fact]
    public void Given_Talents_When_getRemainingTalentPoints_Then_CorrectRemainingTalentPoints()
    {
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, _userId, _world))
      {
        Cost = 0
      });
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, _userId, _world))
      {
        Cost = 1
      });
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, _userId, _world))
      {
        Cost = 2
      });

      Assert.Equal(1, _character.RemainingTalentPoints);
    }

    [Fact]
    public void Given_Talents_When_getSpentTalentPoints_Then_CorrectSpentTalentPoints()
    {
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, _userId, _world))
      {
        Cost = 0
      });
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, _userId, _world))
      {
        Cost = 1
      });
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, _userId, _world))
      {
        Cost = 2
      });
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 1, _userId, _world))
      {
        Cost = 3
      });

      Assert.Equal(6, _character.SpentTalentPoints);
    }

    [Fact]
    public void Given_Talents_When_Validate_Then_Success()
    {
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, _userId, _world))
      {
        Cost = 0
      });
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, _userId, _world))
      {
        Cost = 1
      });
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, _userId, _world))
      {
        Cost = 2
      });

      _character.Validate();
    }
  }
}
