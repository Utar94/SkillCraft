namespace SkillCraft.Core.Characters
{
  [Trait(Traits.Category, Categories.Unit)]
  public class SkillRankTests
  {
    [Theory]
    [InlineData(Skill.Acrobatics, false)]
    [InlineData(Skill.Survival, true)]
    public void Given_Arguments_When_ctor_Then_SkillRank(Skill skill, bool training)
    {
      var skillRank = new SkillRank(skill, training);

      Assert.Equal(skill, skillRank.Skill);
      Assert.Equal(training, skillRank.Training);
      Assert.NotEqual(Guid.Empty, skillRank.Id);
    }

    [Fact]
    public void Given_SkillRank_When_getCost_Then_CorrectCost()
    {
      var trained = new SkillRank(Skill.Acrobatics, true);
      Assert.Equal(1, trained.Cost);

      var untrained = new SkillRank(Skill.Survival, false);
      Assert.Equal(2, untrained.Cost);
    }
  }
}
