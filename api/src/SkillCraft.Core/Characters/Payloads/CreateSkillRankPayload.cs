using Logitar.Validation;

namespace SkillCraft.Core.Characters.Payloads
{
  public class CreateSkillRankPayload
  {
    [Enum(typeof(Skill))]
    public Skill Skill { get; set; }

    public bool Training { get; set; }
  }
}
