using SkillCraft.Core.Conditions;

namespace SkillCraft.Core.Characters
{
  public class CharacterCondition
  {
    public CharacterCondition(Character character, Condition condition)
    {
      Character = character ?? throw new ArgumentNullException(nameof(character));
      CharacterId = character.Id;
      Condition = condition ?? throw new ArgumentNullException(nameof(condition));
      ConditionId = condition.Id;
    }
    private CharacterCondition()
    {
    }

    public Character? Character { get; set; }
    public int CharacterId { get; set; }
    public Condition? Condition { get; set; }
    public int ConditionId { get; set; }

    public int Level { get; set; }
  }
}
