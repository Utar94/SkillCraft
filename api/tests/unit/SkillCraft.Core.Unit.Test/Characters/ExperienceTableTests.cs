namespace SkillCraft.Core.Characters
{
  [Trait(Traits.Category, Categories.Unit)]
  public class ExperienceTableTests
  {
    private readonly ExperienceTable _experienceTable = new();

    [Theory]
    [InlineData(0, 0)]
    [InlineData(42, 0)]
    [InlineData(14472, 7)]
    [InlineData(74099, 12)]
    [InlineData(74100, 13)]
    [InlineData(999999, 20)]
    public void Given_Experience_When_GetLevel_Then_CorrectLevel(int experience, int level)
    {
      Assert.Equal(level, _experienceTable.GetLevel(experience));
    }

    [Fact]
    public void Given_ExperienceOutOfRange_When_GetLevel_Then_ArgumentOutOfRangeException()
    {
      Assert.Equal("experience", Assert.Throws<ArgumentOutOfRangeException>(
        () => _experienceTable.GetLevel(-1)
      ).ParamName);
    }

    [Theory]
    [InlineData(0, 0, 100)]
    [InlineData(1, 100, 300)]
    [InlineData(2, 400, 700)]
    [InlineData(3, 1100, 1300)]
    [InlineData(4, 2400, 2100)]
    [InlineData(5, 4500, 3100)]
    [InlineData(6, 7600, 4300)]
    [InlineData(7, 11900, 5700)]
    [InlineData(8, 17600, 7300)]
    [InlineData(9, 24900, 9100)]
    [InlineData(10, 34000, 11100)]
    [InlineData(11, 45100, 13300)]
    [InlineData(12, 58400, 15700)]
    [InlineData(13, 74100, 18300)]
    [InlineData(14, 92400, 21100)]
    [InlineData(15, 113500, 24100)]
    [InlineData(16, 137600, 27300)]
    [InlineData(17, 164900, 30700)]
    [InlineData(18, 195600, 34300)]
    [InlineData(19, 229900, 38100)]
    [InlineData(20, 268000)]
    public void Given_Level_When_ExperienceTable_Then_CorrectIncrementAndThreshold(int level, int threshold, int? increment = null)
    {
      Assert.Equal(increment, _experienceTable.GetIncrement(level));
      Assert.Equal(threshold, _experienceTable.GetThreshold(level));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(21)]
    public void Given_LevelOutOfRange_When_GetIncrement_Then_ArgumentOutOfRangeException(int level)
    {
      Assert.True(level < 0 || level > ExperienceTable.MaxLevel);

      Assert.Equal("level", Assert.Throws<ArgumentOutOfRangeException>(
        () => _experienceTable.GetIncrement(level)
      ).ParamName);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(21)]
    public void Given_LevelOutOfRange_When_GetThreshold_Then_ArgumentOutOfRangeException(int level)
    {
      Assert.True(level < 0 || level > ExperienceTable.MaxLevel);

      Assert.Equal("level", Assert.Throws<ArgumentOutOfRangeException>(
        () => _experienceTable.GetThreshold(level)
      ).ParamName);
    }
  }
}
