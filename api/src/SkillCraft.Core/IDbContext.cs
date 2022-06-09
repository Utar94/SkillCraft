using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Logging;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core
{
  public interface IDbContext
  {
    DbSet<EventLog> EventLogs { get; }
    DbSet<World> Worlds { get; }

    void CancelChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
  }
}
