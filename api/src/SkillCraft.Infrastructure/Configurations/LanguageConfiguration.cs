using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Languages;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class LanguageConfiguration : EntityBaseConfiguration, IEntityTypeConfiguration<Language>
  {
    public void Configure(EntityTypeBuilder<Language> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);

      builder.Property(x => x.Name).HasMaxLength(256);
      builder.Property(x => x.Script).HasMaxLength(256);
      builder.Property(x => x.TypicalSpeakers).HasMaxLength(256);
    }
  }
}
