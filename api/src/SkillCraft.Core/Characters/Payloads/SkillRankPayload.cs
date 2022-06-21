using Logitar.Validation;

namespace SkillCraft.Core.Characters.Payloads
{
  public class SkillRankPayload
  {
    public Guid? Id { get; set; }

    [Enum(typeof(Skill))]
    public Skill Skill { get; set; }
  }
}
