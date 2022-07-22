using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Races;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class RacialTraitConfiguration : IEntityTypeConfiguration<RacialTrait>
  {
    public void Configure(EntityTypeBuilder<RacialTrait> builder)
    {
      builder.HasKey(x => x.Sid);
      builder.HasIndex(x => x.Id).IsUnique();

      builder.Property(x => x.Id).HasDefaultValueSql("uuid_generate_v4()");
      builder.Property(x => x.Name).HasMaxLength(100);
    }
  }
}
