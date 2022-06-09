using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Customizations;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class CustomizationConfiguration : EntityBaseConfiguration, IEntityTypeConfiguration<Customization>
  {
    public void Configure(EntityTypeBuilder<Customization> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);
      builder.HasIndex(x => x.Type);

      builder.Property(x => x.Name).HasMaxLength(256);
    }
  }
}
