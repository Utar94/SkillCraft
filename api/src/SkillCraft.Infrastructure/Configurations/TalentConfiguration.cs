using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Talents;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class TalentConfiguration : EntityBaseConfiguration, IEntityTypeConfiguration<Talent>
  {
    public void Configure(EntityTypeBuilder<Talent> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.MultipleAcquisition);
      builder.HasIndex(x => x.Name);
      builder.HasIndex(x => x.Tier);

      builder.HasOne(x => x.RequiredTalent).WithMany(x => x.RequiringTalents);

      builder.Property(x => x.MultipleAcquisition).HasDefaultValue(false);
      builder.Property(x => x.Name).HasMaxLength(256);
    }
  }
}
