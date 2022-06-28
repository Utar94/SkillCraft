using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Characters;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class CharacterPowerConfiguration : IEntityTypeConfiguration<CharacterPower>
  {
    public void Configure(EntityTypeBuilder<CharacterPower> builder)
    {
      builder.HasKey(x => new { x.CharacterId, x.PowerId });

      builder.Property(x => x.Cost).HasDefaultValue(0);
    }
  }
}
