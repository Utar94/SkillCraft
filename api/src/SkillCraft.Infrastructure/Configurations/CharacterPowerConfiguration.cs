using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Characters;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class CharacterPowerConfiguration : IEntityTypeConfiguration<CharacterPower>
  {
    public void Configure(EntityTypeBuilder<CharacterPower> builder)
    {
      builder.HasIndex(x => x.Uuid).IsUnique();

      builder.Property(x => x.Cost).HasDefaultValue(0);
      builder.Property(x => x.Uuid).HasDefaultValueSql("uuid_generate_v4()");
    }
  }
}
