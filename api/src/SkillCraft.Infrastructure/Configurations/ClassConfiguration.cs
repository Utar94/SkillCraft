using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Classes;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class ClassConfiguration : EntityBaseConfiguration, IEntityTypeConfiguration<Class>
  {
    public void Configure(EntityTypeBuilder<Class> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);
      builder.HasIndex(x => x.Tier);

      builder.HasOne(x => x.UniqueTalent).WithOne(x => x.Class);

      builder.Property(x => x.Name).HasMaxLength(256);
      builder.Property(x => x.OtherRequirements).HasMaxLength(256);
      builder.Property(x => x.OtherTalentsText).HasMaxLength(256);
    }
  }
}
