using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Powers;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class PowerConfiguration : EntityBaseConfiguration, IEntityTypeConfiguration<Power>
  {
    public void Configure(EntityTypeBuilder<Power> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);
      builder.HasIndex(x => x.Tier);

      builder.Property(x => x.Concentration).HasDefaultValue(false);
      builder.Property(x => x.Focus).HasDefaultValue(false);
      builder.Property(x => x.Incantation).HasDefaultValue(default(IncantationType));
      builder.Property(x => x.Name).HasMaxLength(256);
      builder.Property(x => x.Ritual).HasDefaultValue(false);
      builder.Property(x => x.Somatic).HasDefaultValue(false);
      builder.Property(x => x.Verbal).HasDefaultValue(false);
    }
  }
}
