namespace SkillCraft.Core.Characters.Statistics
{
  public class PrecisionStatistic : StatisticBase
  {
    public PrecisionStatistic(Character character) : base(character)
    {
    }

    public override int Base => Character.Attributes.Coordination.Modifier + 5;
    public override double Increment => (Character.Attributes.Coordination.Modifier + 5) / 10.0;
    protected override Statistic Statistic => Statistic.Precision;
  }
}
