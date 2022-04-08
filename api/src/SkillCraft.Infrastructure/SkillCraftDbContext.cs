#nullable disable
using Logitar.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Aspects;
using SkillCraft.Core.Castes;
using SkillCraft.Core.Characters;
using SkillCraft.Core.Disabilities;
using SkillCraft.Core.Educations;
using SkillCraft.Core.Gifts;
using SkillCraft.Core.Languages;
using SkillCraft.Core.Natures;
using SkillCraft.Core.Talents;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Infrastructure
{
  public class SkillCraftDbContext : IdentityDbContext
  {
    public SkillCraftDbContext(DbContextOptions<SkillCraftDbContext> options) : base(options)
    {
    }

    public DbSet<Aspect> Aspects { get; set; }
    public DbSet<Caste> Castes { get; set; }
    public DbSet<CasteTrait> CasteTraits { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Disability> Disabilities { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Gift> Gifts { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Nature> Natures { get; set; }
    public DbSet<Talent> Talents { get; set; }
    public DbSet<World> Worlds { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.ApplyConfigurationsFromAssembly(typeof(SkillCraftDbContext).Assembly);
    }
  }
}
