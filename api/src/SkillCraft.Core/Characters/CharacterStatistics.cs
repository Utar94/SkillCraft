using SkillCraft.Core.Characters.Statistics;

namespace SkillCraft.Core.Characters
{
  public class CharacterStatistics
  {
    public CharacterStatistics(Character character)
    {
      Constitution = new ConstitutionStatistic(character);
      Initiative = new InitiativeStatistic(character);
      Learning = new LearningStatistic(character);
      Power = new PowerStatistic(character);
      Precision = new PrecisionStatistic(character);
      Repute = new ReputeStatistic(character);
      Strength = new StrengthStatistic(character);
    }

    public ConstitutionStatistic Constitution { get; }
    public InitiativeStatistic Initiative { get; }
    public LearningStatistic Learning { get; }
    public PowerStatistic Power { get; }
    public PrecisionStatistic Precision { get; }
    public ReputeStatistic Repute { get; }
    public StrengthStatistic Strength { get; }
  }
}
