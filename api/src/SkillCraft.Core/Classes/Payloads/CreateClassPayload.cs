using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Core.Classes.Payloads
{
  public class CreateClassPayload : SaveClassPayload
  {
    [Range(0, 3)]
    public int Tier { get; set; }
  }
}
