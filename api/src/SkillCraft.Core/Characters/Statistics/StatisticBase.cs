namespace SkillCraft.Core.Characters.Statistics
{
  public abstract class StatisticBase
  {
    public StatisticBase(Character character)
    {
      Character = character ?? throw new ArgumentNullException(nameof(character));
    }

    protected Character Character { get; }

    protected abstract Statistic Statistic { get; }
    public abstract int Base { get; }
    public abstract double Increment { get; }

    public int Value
    {
      get
      {
        double value = Base;

        foreach (CharacterLevelUp levelUp in Character.LevelUps.Values)
        {
          if (levelUp.Statistics.TryGetValue(Statistic, out double increment))
          {
            value += increment;
          }
        }

        foreach (var bonus in Character.Bonuses)
        {
          if (bonus is StatisticBonus statisticBonus && statisticBonus.Statistic == Statistic)
          {
            value += bonus.Value;
          }
        }

        return (int)value;
      }
    }
  }
}
