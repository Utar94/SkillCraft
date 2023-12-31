﻿using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Aspects;
using SkillCraft.Core.Castes;
using SkillCraft.Core.Characters;
using SkillCraft.Core.Classes;
using SkillCraft.Core.Conditions;
using SkillCraft.Core.Customizations;
using SkillCraft.Core.Educations;
using SkillCraft.Core.Languages;
using SkillCraft.Core.Logging;
using SkillCraft.Core.Natures;
using SkillCraft.Core.Powers;
using SkillCraft.Core.Races;
using SkillCraft.Core.Talents;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core
{
  public interface IDbContext
  {
    DbSet<Aspect> Aspects { get; }
    DbSet<Caste> Castes { get; }
    DbSet<Character> Characters { get; }
    DbSet<Class> Classes { get; }
    DbSet<Condition> Conditions { get; }
    DbSet<Customization> Customizations { get; }
    DbSet<Education> Educations { get; }
    DbSet<EventLog> EventLogs { get; }
    DbSet<Language> Languages { get; }
    DbSet<Nature> Natures { get; }
    DbSet<Power> Powers { get; }
    DbSet<Race> Races { get; }
    DbSet<Talent> Talents { get; }
    DbSet<World> Worlds { get; }

    void CancelChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
  }
}
