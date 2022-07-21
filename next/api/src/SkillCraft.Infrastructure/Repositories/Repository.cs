using SkillCraft.Core;

namespace SkillCraft.Infrastructure.Repositories
{
  internal class Repository<T> : IRepository<T> where T : Aggregate
  {
    private readonly SkillCraftDbContext _dbContext;

    public Repository(SkillCraftDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task SaveAsync(T aggregate, CancellationToken cancellationToken = default)
    {
      ArgumentNullException.ThrowIfNull(aggregate);

      if (aggregate.HasChanges)
      {
        IEnumerable<DbEvent> events = DbEvent.FromChanges(aggregate);

        _dbContext.Events.AddRange(events);
        await _dbContext.SaveChangesAsync(cancellationToken);

        aggregate.ClearChanges();
      }

      if (aggregate.IsDeleted)
      {
        _dbContext.Remove(aggregate);
      }
      else if (aggregate.Sid > 0)
      {
        _dbContext.Update(aggregate);
      }
      else
      {
        _dbContext.Add(aggregate);
      }

      await _dbContext.SaveChangesAsync(cancellationToken);
    }
  }
}
