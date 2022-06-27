using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Characters.Payloads
{
  public class CharacterConditionPayload
  {
    public Guid ConditionId { get; set; }

    [Range(0, 6)]
    public int Level { get; set; }
  }
}
