namespace SkillCraft.Core.Races.Payload
{
  public class CreateRacePayload : SaveRacePayload
  {
    public Guid? ParentId { get; set; }
  }
}
