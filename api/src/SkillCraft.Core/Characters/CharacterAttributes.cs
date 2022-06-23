namespace SkillCraft.Core.Characters
{
  public class CharacterAttributes
  {
    public CharacterAttributes(Character character)
    {
      Agility = new(Attribute.Agility, character);
      Coordination = new(Attribute.Coordination, character);
      Intellect = new(Attribute.Intellect, character);
      Mind = new(Attribute.Mind, character);
      Presence = new(Attribute.Presence, character);
      Sensitivity = new(Attribute.Sensitivity, character);
      Vigor = new(Attribute.Vigor, character);
    }

    public CharacterAttribute Agility { get; }
    public CharacterAttribute Coordination { get; }
    public CharacterAttribute Intellect { get; }
    public CharacterAttribute Mind { get; }
    public CharacterAttribute Presence { get; }
    public CharacterAttribute Sensitivity { get; }
    public CharacterAttribute Vigor { get; }
  }
}
