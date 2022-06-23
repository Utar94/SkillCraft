namespace SkillCraft.Core.Characters.Statistics
{
  public class LearningStatistic : StatisticBase
  {
    public LearningStatistic(Character character) : base(character)
    {
    }

    public override int Base
    {
      get
      {
        int @base = 2 * Character.Attributes.Intellect.Modifier + 5;

        return @base < 5 ? 5 : @base;
      }
    }
    public override double Increment
    {
      get
      {
        int increment = 2 + Character.Attributes.Intellect.Modifier;

        return increment < 1 ? 1 : increment;
      }
    }
    protected override Statistic Statistic => Statistic.Learning;
  }
}
