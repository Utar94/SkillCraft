namespace SkillCraft.Core.Characters
{
  public class CharacterLevelUp
  {
    public CharacterLevelUp(Attribute attribute)
    {
      Attribute = attribute;
    }
    private CharacterLevelUp()
    {
    }

    public Attribute Attribute { get; private set; }
    public Dictionary<Statistic, double> Statistics { get; private set; } = new();

    public void CalculateStatistics(CharacterStatistics statistics)
    {
      ArgumentNullException.ThrowIfNull(statistics);

      Statistics.Add(Statistic.Constitution, statistics.Constitution.Increment);
      Statistics.Add(Statistic.Initiative, statistics.Initiative.Increment);
      Statistics.Add(Statistic.Learning, statistics.Learning.Increment);
      Statistics.Add(Statistic.Power, statistics.Power.Increment);
      Statistics.Add(Statistic.Precision, statistics.Precision.Increment);
      Statistics.Add(Statistic.Repute, statistics.Repute.Increment);
      Statistics.Add(Statistic.Strength, statistics.Strength.Increment);
    }
  }
}
