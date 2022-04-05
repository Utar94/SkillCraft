using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Disabilities;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class DisabilityConfiguration : AggregateConfiguration, IEntityTypeConfiguration<Disability>
  {
    public void Configure(EntityTypeBuilder<Disability> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);

      builder.Property(x => x.Name).HasMaxLength(100);
    }
  }
}
