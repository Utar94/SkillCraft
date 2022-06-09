using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class WorldConfiguration : EntityBaseConfiguration, IEntityTypeConfiguration<World>
  {
    public void Configure(EntityTypeBuilder<World> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Alias).IsUnique();
      builder.HasIndex(x => x.Name);

      builder.Property(x => x.Alias).HasMaxLength(256);
      builder.Property(x => x.Name).HasMaxLength(256);
    }
  }
}
