using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Characters;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class CharacterTalentConfiguration : IEntityTypeConfiguration<CharacterTalent>
  {
    public void Configure(EntityTypeBuilder<CharacterTalent> builder)
    {
      builder.HasKey(x => new { x.CharacterId, x.TalentId });

      builder.Property(x => x.Cost).HasDefaultValue(0);
    }
  }
}
