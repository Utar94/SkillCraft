using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core;
using SkillCraft.Core.Characters;
using SkillCraft.Infrastructure.Entities;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class CharacterConfiguration : EntityBaseConfiguration, IEntityTypeConfiguration<Character>
  {
    public void Configure(EntityTypeBuilder<Character> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);
      builder.HasIndex(x => x.Player);

      builder.HasMany(x => x.Customizations).WithMany(x => x.Characters)
        .UsingEntity<CharacterCustomization>(builder => builder.HasKey(x => new { x.CharacterId, x.CustomizationId }));
      builder.HasMany(x => x.Languages).WithMany(x => x.Characters)
        .UsingEntity<CharacterLanguage>(builder => builder.HasKey(x => new { x.CharacterId, x.LanguageId }));
      builder.HasOne(x => x.Aspect1).WithMany(x => x.Characters1);
      builder.HasOne(x => x.Aspect2).WithMany(x => x.Characters2);

      builder.Ignore(x => x.Bonuses);
      builder.Property(x => x.BonusesSerialized)
        .HasColumnName(nameof(Character.Bonuses))
        .HasColumnType("jsonb[]");
      builder.Ignore(x => x.Creation);
      builder.Property(x => x.CreationSerialized)
        .HasColumnName(nameof(Character.Creation))
        .HasColumnType("jsonb");
      builder.Ignore(x => x.LevelUps);
      builder.Property(x => x.LevelUpsSerialized)
        .HasColumnName(nameof(Character.LevelUps))
        .HasColumnType("jsonb");
      builder.Ignore(x => x.SkillRanks);
      builder.Property(x => x.SkillRanksSerialized)
        .HasColumnName(nameof(Character.SkillRanks))
        .HasColumnType("jsonb");

      builder.Property(x => x.Age).HasDefaultValue(0);
      builder.Property(x => x.BloodAlcoholContent).HasDefaultValue(0);
      builder.Property(x => x.Experience).HasDefaultValue(0);
      builder.Property(x => x.Intoxication).HasDefaultValue(0);
      builder.Property(x => x.Name).HasMaxLength(100);
      builder.Property(x => x.Player).HasMaxLength(100);
      builder.Property(x => x.Size).HasDefaultValue(SizeCategory.Medium);
      builder.Property(x => x.Stamina).HasDefaultValue(0);
      builder.Property(x => x.Stature).HasDefaultValue(0.0);
      builder.Property(x => x.Vitality).HasDefaultValue(0);
      builder.Property(x => x.Weight).HasDefaultValue(0.0);
    }
  }
}
