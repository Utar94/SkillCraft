#nullable disable
using Logitar.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SkillCraft.Core;
using SkillCraft.Core.Aspects;
using SkillCraft.Core.Castes;
using SkillCraft.Core.Customizations;
using SkillCraft.Core.Educations;
using SkillCraft.Core.Languages;
using SkillCraft.Core.Logging;
using SkillCraft.Core.Natures;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Infrastructure
{
  public class SkillCraftDbContext : IdentityDbContext, IDbContext
  {
    public SkillCraftDbContext(DbContextOptions<SkillCraftDbContext> options) : base(options)
    {
    }

    public DbSet<Aspect> Aspects { get; set; }
    public DbSet<Caste> Castes { get; set; }
    public DbSet<Customization> Customizations { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<EventLog> EventLogs { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Nature> Natures { get; set; }
    public DbSet<World> Worlds { get; set; }

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
