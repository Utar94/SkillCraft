using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SkillCraft.Core;
using SkillCraft.Core.Aspects;
using SkillCraft.Core.Customizations;
using SkillCraft.Core.Natures;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Infrastructure
{
  internal class SkillCraftDbContext : DbContext
  {
    private readonly IConfiguration _configuration;

    public SkillCraftDbContext(IConfiguration configuration, DbContextOptions<SkillCraftDbContext> options)
      : base(options)
    {
      _configuration = configuration;
    }

    public DbSet<Aspect> Aspects { get; private set; } = null!;
    public DbSet<Customization> Customizations { get; private set; } = null!;
    public DbSet<DbEvent> Events { get; private set; } = null!;
    public DbSet<Nature> Natures { get; private set; } = null!;
    public DbSet<World> Worlds { get; private set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseNpgsql(_configuration.GetValue<string>($"POSTGRESQLCONNSTR_{nameof(SkillCraftDbContext)}"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(SkillCraftDbContext).Assembly);

      modelBuilder.HasPostgresExtension("uuid-ossp");

      modelBuilder.Ignore<Event>();
    }
  }
}
