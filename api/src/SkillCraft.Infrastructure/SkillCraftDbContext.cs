#nullable disable
using Logitar.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Infrastructure
{
  public class SkillCraftDbContext : IdentityDbContext
  {
    public SkillCraftDbContext(DbContextOptions<SkillCraftDbContext> options) : base(options)
    {
    }

    public DbSet<World> Worlds { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.ApplyConfigurationsFromAssembly(typeof(SkillCraftDbContext).Assembly);
    }
  }
}
