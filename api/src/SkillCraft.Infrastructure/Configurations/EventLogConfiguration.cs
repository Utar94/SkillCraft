using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using SkillCraft.Core.Logging;

namespace SkillCraft.Infrastructure.Configurations
{
  internal class EventLogConfiguration : EntityBaseConfiguration, IEntityTypeConfiguration<EventLog>
  {
    public void Configure(EntityTypeBuilder<EventLog> builder)
    {
      base.Configure(builder);

      builder.Ignore(x => x.EntityType);
      builder.Property(x => x.EntityTypeName)
        .HasColumnName(nameof(EventLog.EntityType))
        .HasMaxLength(256);

      builder.Ignore(x => x.Errors);
      builder.Property(x => x.SerializedErrors)
        .HasColumnName(nameof(EventLog.Errors))
        .HasColumnType("jsonb");

      builder.Property(x => x.HasErrors).HasDefaultValue(false);
      builder.Property(x => x.IsCompleted).HasDefaultValue(false);
      builder.Property(x => x.Level).HasDefaultValue(default(LogLevel));
      builder.Property(x => x.Method).HasMaxLength(16);
      builder.Property(x => x.Name).HasMaxLength(256);
      builder.Property(x => x.StartedAt).HasDefaultValueSql("now()");
      builder.Property(x => x.Url).HasMaxLength(2048);
    }
  }
}
