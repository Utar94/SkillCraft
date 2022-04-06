using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Characters;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class CharacterConfiguration : AggregateConfiguration, IEntityTypeConfiguration<Character>
  {
    public void Configure(EntityTypeBuilder<Character> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);

      builder.Property(x => x.Age).HasDefaultValue(0);
      builder.Property(x => x.BloodAlcoholContent).HasDefaultValue(0);
      builder.Property(x => x.Experience).HasDefaultValue(0);
      builder.Property(x => x.Intoxication).HasDefaultValue(0);
      builder.Property(x => x.Name).HasMaxLength(100);
      builder.Property(x => x.Player).HasMaxLength(100);
      builder.Property(x => x.Stamina).HasDefaultValue(0);
      builder.Property(x => x.Stature).HasDefaultValue(0.0);
      builder.Property(x => x.Weight).HasDefaultValue(0.0);
      builder.Property(x => x.Vitality).HasDefaultValue(0);
    }
  }
}
