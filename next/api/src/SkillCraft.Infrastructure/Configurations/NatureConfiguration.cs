using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Natures;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class NatureConfiguration : AggregateConfiguration<Nature>, IEntityTypeConfiguration<Nature>
  {
    public override void Configure(EntityTypeBuilder<Nature> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Attribute);
      builder.HasIndex(x => x.Name);

      builder.Property(x => x.Name).HasMaxLength(100);
    }
  }
}
