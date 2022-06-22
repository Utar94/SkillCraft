namespace SkillCraft.Core.Characters.Payloads
{
  public class SaveCharacterStep3Payload
  {
    public Guid NatureId { get; set; }

    public IEnumerable<Guid>? CustomizationIds { get; set; }
  }
}
