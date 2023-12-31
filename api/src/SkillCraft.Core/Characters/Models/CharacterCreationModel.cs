﻿namespace SkillCraft.Core.Characters.Models
{
  public class CharacterCreationModel
  {
    public AttributeBasesModel? AttributeBases { get; set; }

    public Attribute BestAttribute { get; set; }
    public Attribute WorstAttribute { get; set; }
    public Attribute MandatoryAttribute1 { get; set; }
    public Attribute MandatoryAttribute2 { get; set; }
    public Attribute OptionalAttribute1 { get; set; }
    public Attribute OptionalAttribute2 { get; set; }

    public int? Step { get; set; }
  }
}
