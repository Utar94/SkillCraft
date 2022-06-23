namespace SkillCraft.Core.Characters.Statistics
{
  public class StrengthStatistic : StatisticBase
  {
    public StrengthStatistic(Character character) : base(character)
    {
    }

    public override int Base => Character.Attributes.Agility.Modifier + 5;
    public override double Increment => (Character.Attributes.Agility.Modifier + 5) / 10.0;
    protected override Statistic Statistic => Statistic.Strength;
  }
}
