using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core.Races;
using SkillCraft.Infrastructure.Entities;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class RaceConfiguration : EntityBaseConfiguration, IEntityTypeConfiguration<Race>
  {
    public void Configure(EntityTypeBuilder<Race> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);

      builder.HasOne(x => x.Parent).WithMany(x => x.Children);
      builder.HasMany(x => x.Languages).WithMany(x => x.Races)
        .UsingEntity<RaceLanguage>(builder => builder.HasKey(x => new { x.RaceId, x.LanguageId }));

      builder.Ignore(x => x.Attributes);
      builder.Property(x => x.AttributesSerialized)
        .HasColumnName(nameof(Race.Attributes))
        .HasMaxLength(100);
      builder.Ignore(x => x.Names);
      builder.Property(x => x.NamesSerialized)
        .HasColumnName(nameof(Race.Names))
        .HasColumnType("jsonb");
      builder.Ignore(x => x.Speeds);
      builder.Property(x => x.SpeedsSerialized)
        .HasColumnName(nameof(Race.Speeds))
        .HasMaxLength(50);
      builder.Ignore(x => x.Traits);
      builder.Property(x => x.TraitsSerialized)
        .HasColumnName(nameof(Race.Traits))
        .HasColumnType("jsonb");

      builder.Property(x => x.Name).HasMaxLength(256);
      builder.Property(x => x.StatureRoll).HasMaxLength(10);
      builder.Property(x => x.WeightRolls).HasColumnType("character varying(10)[]");
    }
  }
}
