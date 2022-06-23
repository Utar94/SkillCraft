namespace SkillCraft.Core.Characters
{
  public class CharacterCreation
  {
    public Dictionary<Attribute, int> AttributeBases { get; private set; } = new();

    public Attribute BestAttribute { get; set; }
    public Attribute WorstAttribute { get; set; }
    public Attribute MandatoryAttribute1 { get; set; }
    public Attribute MandatoryAttribute2 { get; set; }
    public Attribute OptionalAttribute1 { get; set; }
    public Attribute OptionalAttribute2 { get; set; }

    public int Step { get; set; }
  }
}
