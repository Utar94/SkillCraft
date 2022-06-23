namespace SkillCraft.Core.Characters.Statistics
{
  public class PowerStatistic : StatisticBase
  {
    public PowerStatistic(Character character) : base(character)
    {
    }

    public override int Base => Character.Attributes.Mind.Modifier + 5;
    public override double Increment => (Character.Attributes.Mind.Modifier + 5) / 10.0;
    protected override Statistic Statistic => Statistic.Power;
  }
}
