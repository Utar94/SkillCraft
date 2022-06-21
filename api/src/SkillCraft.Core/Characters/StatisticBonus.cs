namespace SkillCraft.Core.Characters
{
  public class StatisticBonus : BonusBase
  {
    public StatisticBonus(Statistic statistic, Guid userId) : base(userId)
    {
      Statistic = statistic;
    }

    public Statistic Statistic { get; private set; }
  }
}
