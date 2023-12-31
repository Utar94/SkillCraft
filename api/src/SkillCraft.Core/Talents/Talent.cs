﻿using SkillCraft.Core.Characters;
using SkillCraft.Core.Classes;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Talents
{
  public class Talent : EntityBase
  {
    public Talent(int tier, Guid userId, World world) : base(userId)
    {
      Tier = tier;
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Talent() : base()
    {
    }

    public World? World { get; set; }
    public int WorldId { get; set; }

    public bool MultipleAcquisition { get; set; }
    public Talent? RequiredTalent { get; set; }
    public int? RequiredTalentId { get; set; }
    public Skill? Skill { get; set; }
    public int Tier { get; set; }
    public int Cost => Tier + 2;

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public Class? Class { get; set; }

    public ICollection<CharacterTalent> CharacterTalents { get; set; } = new List<CharacterTalent>();
    public ICollection<ClassTalent> ClassTalents { get; set; } = new List<ClassTalent>();
    public ICollection<TalentOption> Options { get; set; } = new List<TalentOption>();
    public ICollection<Talent> RequiringTalents { get; set; } = new List<Talent>();

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
