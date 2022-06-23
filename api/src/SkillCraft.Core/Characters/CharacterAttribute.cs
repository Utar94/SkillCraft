namespace SkillCraft.Core.Characters
{
  public class CharacterAttribute
  {
    private readonly Attribute _attribute;
    private readonly Character _character;

    public CharacterAttribute(Attribute attribute, Character character)
    {
      _attribute = attribute;
      _character = character ?? throw new ArgumentNullException(nameof(character));
    }

    public int Score
    {
      get
      {
        int score = 0;

        CharacterCreation? creation = _character.Creation;
        if (creation != null)
        {
          creation.AttributeBases.TryGetValue(_attribute, out score);

          if (creation.BestAttribute == _attribute)
            score += 3;
          if (creation.MandatoryAttribute1 == _attribute)
            score += 2;
          if (creation.MandatoryAttribute2 == _attribute)
            score += 2;
          if (creation.WorstAttribute == _attribute)
            score += 1;
          if (creation.OptionalAttribute1 == _attribute)
            score += 1;
          if (creation.OptionalAttribute2 == _attribute)
            score += 1;
        }

        if (_character.Race?.Attributes.TryGetValue(_attribute, out int racialBonus) == true)
        {
          score += racialBonus;
        }

        if (_character.Nature?.Attribute == _attribute)
        {
          score += 1;
        }

        score += _character.LevelUps.Count(x => x.Value.Attribute == _attribute);

        foreach (BonusBase bonus in _character.Bonuses)
        {
          if (bonus is AttributeBonus attributeBonus && attributeBonus.Attribute == _attribute)
          {
            score += bonus.Value;
          }
        }

        return score;
      }
    }
    public int Modifier => Score / 2 - 5;
  }
}
