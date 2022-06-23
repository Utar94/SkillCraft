namespace SkillCraft.Core.Characters.Statistics
{
  public class ConstitutionStatistic : StatisticBase
  {
    public ConstitutionStatistic(Character character) : base(character)
    {
    }

    public override int Base => 5 * (Character.Attributes.Vigor.Modifier + 5);
    public override double Increment => Character.Attributes.Vigor.Modifier + 5;
    protected override Statistic Statistic => Statistic.Constitution;
  }
}
