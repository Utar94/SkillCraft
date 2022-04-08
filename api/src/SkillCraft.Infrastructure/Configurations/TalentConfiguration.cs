using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Talents;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class TalentConfiguration : AggregateConfiguration, IEntityTypeConfiguration<Talent>
  {
    public void Configure(EntityTypeBuilder<Talent> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);

      builder.Property(x => x.MultipleAcquisitions).HasDefaultValue(false);
      builder.Property(x => x.Name).HasMaxLength(100);
      builder.Property(x => x.Tier).HasDefaultValue(0);
    }
  }
}
