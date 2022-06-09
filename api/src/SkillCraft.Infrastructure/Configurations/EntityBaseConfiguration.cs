using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class EntityBaseConfiguration
  {
    public virtual void Configure<T>(EntityTypeBuilder<T> builder) where T : EntityBase
    {
      builder.HasIndex(x => x.CreatedById);
      builder.HasIndex(x => x.Deleted);
      builder.HasIndex(x => x.Uuid).IsUnique();

      builder.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
      builder.Property(x => x.Deleted).HasDefaultValue(false);
      builder.Property(x => x.Uuid).HasDefaultValueSql("uuid_generate_v4()");
      builder.Property(x => x.Version).HasDefaultValue(0);
    }
  }
}
