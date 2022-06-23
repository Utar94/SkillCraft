namespace SkillCraft.Core.Characters.Statistics
{
  public class ReputeStatistic : StatisticBase
  {
    public ReputeStatistic(Character character) : base(character)
    {
    }

    public override int Base => Character.Attributes.Presence.Modifier;
    public override double Increment => Character.Attributes.Presence.Score / 20.0;
    protected override Statistic Statistic => Statistic.Repute;
  }
}
