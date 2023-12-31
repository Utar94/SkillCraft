﻿#nullable disable
using Logitar.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SkillCraft.Core;
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
using SkillCraft.Infrastructure.Entities;

namespace SkillCraft.Infrastructure
{
  public class SkillCraftDbContext : IdentityDbContext, IDbContext
  {
    public SkillCraftDbContext(DbContextOptions<SkillCraftDbContext> options) : base(options)
    {
    }

    public DbSet<Aspect> Aspects { get; set; }
    public DbSet<Caste> Castes { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Condition> Conditions { get; set; }
    public DbSet<Customization> Customizations { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<EventLog> EventLogs { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Nature> Natures { get; set; }
    public DbSet<Power> Powers { get; set; }
    public DbSet<Race> Races { get; set; }
    public DbSet<Talent> Talents { get; set; }
    public DbSet<World> Worlds { get; set; }

    internal DbSet<CharacterCondition> CharacterConditions { get; set; }
    internal DbSet<CharacterCustomization> CharacterCustomizations { get; set; }
    internal DbSet<CharacterLanguage> CharacterLanguages { get; set; }
    internal DbSet<CharacterPower> CharacterPowers { get; set; }
    internal DbSet<CharacterTalent> CharacterTalents { get; set; }
    internal DbSet<ClassTalent> ClassTalents { get; set; }
    internal DbSet<RaceLanguage> RaceLanguages { get; set; }
    internal DbSet<TalentOption> TalentOptions { get; set; }

    public void CancelChanges()
    {
      IEnumerable<EntityEntry> entries = ChangeTracker
          .Entries()
          .Where(x => x.State != EntityState.Unchanged);

      foreach (EntityEntry entry in entries)
      {
        switch (entry.State)
        {
          case EntityState.Added:
            entry.State = EntityState.Detached;
            break;
          case EntityState.Deleted:
            entry.State = EntityState.Unchanged;
            break;
          case EntityState.Modified:
            entry.CurrentValues.SetValues(entry.OriginalValues);
            entry.State = EntityState.Unchanged;
            break;
        }
      }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.ApplyConfigurationsFromAssembly(typeof(SkillCraftDbContext).Assembly);

      builder.HasPostgresExtension("uuid-ossp");
    }
  }
}
