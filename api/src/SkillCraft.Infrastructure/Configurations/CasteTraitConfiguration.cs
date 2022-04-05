using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Castes;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class CasteTraitConfiguration : IEntityTypeConfiguration<CasteTrait>
  {
    public void Configure(EntityTypeBuilder<CasteTrait> builder)
    {
      builder.HasIndex(x => x.Key).IsUnique();

      builder.Property(x => x.Key).HasDefaultValueSql("uuid_generate_v4()");
      builder.Property(x => x.Name).HasMaxLength(100);
    }
  }
}
