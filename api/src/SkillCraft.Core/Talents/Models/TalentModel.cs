﻿using SkillCraft.Core.Models;

namespace SkillCraft.Core.Talents.Models
{
  public class TalentModel : EntityBaseModel
  {
    public bool MultipleAcquisition { get; set; }
    public TalentModel? RequiredTalent { get; set; }
    public Skill? Skill { get; set; }
    public int Tier { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public IEnumerable<TalentOptionModel> Options { get; set; } = null!;
  }
}
