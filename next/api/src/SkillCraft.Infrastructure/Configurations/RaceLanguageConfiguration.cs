using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Infrastructure.Entities;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class RaceLanguageConfiguration : IEntityTypeConfiguration<RaceLanguage>
  {
    public void Configure(EntityTypeBuilder<RaceLanguage> builder)
    {
      builder.HasKey(x => new { x.RaceSid, x.LanguageSid });
    }
  }
}
