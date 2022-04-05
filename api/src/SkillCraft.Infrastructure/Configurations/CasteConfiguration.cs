using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Castes;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class CasteConfiguration : AggregateConfiguration, IEntityTypeConfiguration<Caste>
  {
    public void Configure(EntityTypeBuilder<Caste> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);

      builder.Property(x => x.Name).HasMaxLength(100);
      builder.Property(x => x.WealthRoll).HasMaxLength(10);
    }
  }
}
