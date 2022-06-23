namespace SkillCraft.Core.Characters.Statistics
{
  public class InitiativeStatistic : StatisticBase
  {
    public InitiativeStatistic(Character character) : base(character)
    {
    }

    public override int Base => Character.Attributes.Sensitivity.Modifier;
    public override double Increment => Character.Attributes.Sensitivity.Score / 40.0;
    protected override Statistic Statistic => Statistic.Initiative;
  }
}
