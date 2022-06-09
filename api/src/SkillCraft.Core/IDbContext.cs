using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Aspects;
using SkillCraft.Core.Customizations;
using SkillCraft.Core.Logging;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core
{
  public interface IDbContext
  {
    DbSet<Aspect> Aspects { get; }
    DbSet<Customization> Customizations { get; }
    DbSet<EventLog> EventLogs { get; }
    DbSet<World> Worlds { get; }

    void CancelChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
  }
}
