using Logitar.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SkillCraft.Infrastructure
{
  public class SkillCraftDbContext : IdentityDbContext
  {
    public SkillCraftDbContext(DbContextOptions<SkillCraftDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.ApplyConfigurationsFromAssembly(typeof(SkillCraftDbContext).Assembly);
    }
  }
}
