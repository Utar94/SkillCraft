using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Powers;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class PowerConfiguration : AggregateConfiguration<Power>, IEntityTypeConfiguration<Power>
  {
    public override void Configure(EntityTypeBuilder<Power> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);
      builder.HasIndex(x => x.Tier);

      builder.Property(x => x.Incantation).HasDefaultValue(default(IncantationType));
      builder.Property(x => x.IsConcentration).HasDefaultValue(false);
      builder.Property(x => x.IsFocus).HasDefaultValue(false);
      builder.Property(x => x.IsRitual).HasDefaultValue(false);
      builder.Property(x => x.IsSomatic).HasDefaultValue(false);
      builder.Property(x => x.IsVerbal).HasDefaultValue(false);
      builder.Property(x => x.Name).HasMaxLength(100);
    }
  }
}
