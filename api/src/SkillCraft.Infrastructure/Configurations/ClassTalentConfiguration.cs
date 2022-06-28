using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Classes;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class ClassTalentConfiguration : IEntityTypeConfiguration<ClassTalent>
  {
    public void Configure(EntityTypeBuilder<ClassTalent> builder)
    {
      builder.HasKey(x => new { x.ClassId, x.TalentId });

      builder.Property(x => x.Mandatory).HasDefaultValue(false);
    }
  }
}
