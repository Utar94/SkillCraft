using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Gifts;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class GiftConfiguration : AggregateConfiguration, IEntityTypeConfiguration<Gift>
  {
    public void Configure(EntityTypeBuilder<Gift> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);

      builder.Property(x => x.Name).HasMaxLength(100);
    }
  }
}
