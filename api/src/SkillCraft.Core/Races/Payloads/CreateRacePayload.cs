namespace SkillCraft.Core.Races.Payloads
{
  public class CreateRacePayload : SaveRacePayload
  {
    public Guid? ParentId { get; set; }
  }
}
