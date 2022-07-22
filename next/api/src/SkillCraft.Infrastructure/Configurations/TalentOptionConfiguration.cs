using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Talents;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class TalentOptionConfiguration : IEntityTypeConfiguration<TalentOption>
  {
    public void Configure(EntityTypeBuilder<TalentOption> builder)
    {
      builder.HasKey(x => x.Sid);
      builder.HasIndex(x => x.Id).IsUnique();

      builder.Property(x => x.Id).HasDefaultValueSql("uuid_generate_v4()");
      builder.Property(x => x.Name).HasMaxLength(100);
    }
  }
}
