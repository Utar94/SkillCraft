using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Castes;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class CasteConfiguration : AggregateConfiguration<Caste>, IEntityTypeConfiguration<Caste>
  {
    public override void Configure(EntityTypeBuilder<Caste> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);
      builder.HasIndex(x => x.Skill);

      builder.Property(x => x.Name).HasMaxLength(100);
      builder.Property(x => x.WealthRoll).HasMaxLength(10);
    }
  }
}
