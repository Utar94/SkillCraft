using SkillCraft.Core.Fakers;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Talents
{
  [Trait(Traits.Category, Categories.Unit)]
  public class TalentTests
  {
    private static readonly WorldFaker _worldFaker = new();

    private readonly World _world = _worldFaker.Generate();

    private Guid UserId => _world.CreatedById;

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Given_Tier_When_getCost_Then_CorrectCost(int tier)
    {
      var talent = new Talent(tier, UserId, _world);
      Assert.Equal(tier + 2, talent.Cost);
    }
  }
}
