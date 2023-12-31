﻿using SkillCraft.Core.Characters;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Aspects
{
  public class Aspect : EntityBase
  {
    public Aspect(Guid userId, World world) : base(userId)
    {
      World = world ?? throw new ArgumentNullException(nameof(world));
      WorldId = world.Id;
    }
    private Aspect() : base()
    {
    }

    public World? World { get; set; }
    public int WorldId { get; set; }

    public string? Description { get; set; }
    public string Name { get; set; } = null!;

    public Attribute MandatoryAttribute1 { get; set; }
    public Attribute MandatoryAttribute2 { get; set; }
    public Attribute OptionalAttribute1 { get; set; }
    public Attribute OptionalAttribute2 { get; set; }
    public Skill Skill1 { get; set; }
    public Skill Skill2 { get; set; }

    public ICollection<Character> Characters1 { get; set; } = new List<Character>();
    public ICollection<Character> Characters2 { get; set; } = new List<Character>();

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
