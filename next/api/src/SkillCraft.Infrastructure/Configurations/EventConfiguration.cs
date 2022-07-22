using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Infrastructure.Entities;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class EventConfiguration : IEntityTypeConfiguration<DbEvent>
  {
    public void Configure(EntityTypeBuilder<DbEvent> builder)
    {
      builder.HasKey(x => x.Sid);
      builder.HasIndex(x => x.Id).IsUnique();
      builder.HasIndex(x => new { x.AggregateType, x.AggregateId });

      builder.Property(x => x.AggregateType).HasMaxLength(256);
      builder.Property(x => x.EventData).HasColumnType("jsonb");
      builder.Property(x => x.EventType).HasMaxLength(256);
      builder.Property(x => x.Id).HasDefaultValueSql("uuid_generate_v4()");
    }
  }
}
