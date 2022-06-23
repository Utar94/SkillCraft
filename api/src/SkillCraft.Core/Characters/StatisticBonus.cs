namespace SkillCraft.Core.Characters
{
  public class StatisticBonus : BonusBase
  {
    public StatisticBonus(Statistic statistic)
    {
      Statistic = statistic;
    }

    public Statistic Statistic { get; private set; }
  }
}
