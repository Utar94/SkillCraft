using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Languages;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class LanguageConfiguration : AggregateConfiguration, IEntityTypeConfiguration<Language>
  {
    public void Configure(EntityTypeBuilder<Language> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);
      builder.HasIndex(x => x.Script);
      builder.HasIndex(x => x.TypicalSpeakers);

      builder.Property(x => x.Exotic).HasDefaultValue(false);
      builder.Property(x => x.Name).HasMaxLength(100);
      builder.Property(x => x.Script).HasMaxLength(100);
      builder.Property(x => x.TypicalSpeakers).HasMaxLength(100);
    }
  }
}
