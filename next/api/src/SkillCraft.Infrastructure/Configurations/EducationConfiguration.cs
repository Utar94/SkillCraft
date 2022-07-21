using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Educations;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class EducationConfiguration : AggregateConfiguration<Education>, IEntityTypeConfiguration<Education>
  {
    public override void Configure(EntityTypeBuilder<Education> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);
      builder.HasIndex(x => x.Skill);

      builder.Property(x => x.Name).HasMaxLength(100);
      builder.Property(x => x.WealthMultiplier).HasDefaultValue(0);
    }
  }
}
