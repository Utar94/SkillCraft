using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Aspects;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class AspectConfiguration : AggregateConfiguration<Aspect>, IEntityTypeConfiguration<Aspect>
  {
    public override void Configure(EntityTypeBuilder<Aspect> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);

      builder.Property(x => x.Name).HasMaxLength(100);
    }
  }
}
