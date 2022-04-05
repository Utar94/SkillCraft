using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Educations;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class EducationConfiguration : AggregateConfiguration, IEntityTypeConfiguration<Education>
  {
    public void Configure(EntityTypeBuilder<Education> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);

      builder.Property(x => x.Name).HasMaxLength(100);
    }
  }
}
