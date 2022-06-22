using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Talents;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class TalentOptionConfiguration : IEntityTypeConfiguration<TalentOption>
  {
    public void Configure(EntityTypeBuilder<TalentOption> builder)
    {
      builder.HasIndex(x => x.Uuid).IsUnique();

      builder.Property(x => x.Name).HasMaxLength(128);
      builder.Property(x => x.Uuid).HasDefaultValueSql("uuid_generate_v4()");
    }
  }
}
