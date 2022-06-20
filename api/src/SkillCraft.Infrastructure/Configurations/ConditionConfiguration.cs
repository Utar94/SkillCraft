using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Conditions;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class ConditionConfiguration : EntityBaseConfiguration, IEntityTypeConfiguration<Condition>
  {
    public void Configure(EntityTypeBuilder<Condition> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);

      builder.Property(x => x.MaxLevel).HasDefaultValue(0);
      builder.Property(x => x.Name).HasMaxLength(256);
    }
  }
}
