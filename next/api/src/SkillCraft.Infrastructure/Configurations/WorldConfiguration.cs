using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class WorldConfiguration : AggregateConfiguration<World>, IEntityTypeConfiguration<World>
  {
    public override void Configure(EntityTypeBuilder<World> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Alias);
      builder.HasIndex(x => x.AliasNormalized).IsUnique();
      builder.HasIndex(x => x.CreatedById);
      builder.HasIndex(x => x.Name);

      builder.Property(x => x.Alias).HasMaxLength(100);
      builder.Property(x => x.AliasNormalized).HasMaxLength(100);
      builder.Property(x => x.Name).HasMaxLength(100);
    }
  }
}
