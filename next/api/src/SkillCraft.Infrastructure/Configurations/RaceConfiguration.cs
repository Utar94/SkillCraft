using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Core;
using SkillCraft.Core.Races;
using SkillCraft.Infrastructure.Entities;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class RaceConfiguration : AggregateConfiguration<Race>, IEntityTypeConfiguration<Race>
  {
    public override void Configure(EntityTypeBuilder<Race> builder)
    {
      base.Configure(builder);

      builder.HasOne(x => x.Parent).WithMany(x => x.People);
      builder.HasMany(x => x.Languages).WithMany(x => x.Races).UsingEntity<RaceLanguage>();

      builder.HasIndex(x => x.Name);
      builder.HasIndex(x => x.Size);

      builder.Ignore(x => x.Attributes);
      builder.Ignore(x => x.Names);
      builder.Ignore(x => x.Speeds);

      builder.Property(x => x.AttributesSerialized).HasColumnName(nameof(Race.Attributes)).HasMaxLength(100);
      builder.Property(x => x.ExtraAttributes).HasDefaultValue(0);
      builder.Property(x => x.ExtraLanguages).HasDefaultValue(0);
      builder.Property(x => x.Name).HasMaxLength(100);
      builder.Property(x => x.NamesSerialized).HasColumnName(nameof(Race.Names)).HasColumnType("jsonb");
      builder.Property(x => x.Size).HasDefaultValue(default(SizeCategory));
      builder.Property(x => x.SpeedsSerialized).HasColumnName(nameof(Race.Speeds)).HasMaxLength(100);
      builder.Property(x => x.StatureRoll).HasMaxLength(10);
      builder.Property(x => x.WeightRolls).HasColumnType("character varying(10)[]");
    }
  }
}
