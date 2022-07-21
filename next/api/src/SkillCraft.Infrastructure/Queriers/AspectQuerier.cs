using Microsoft.EntityFrameworkCore;
using SkillCraft.Core;
using SkillCraft.Core.Aspects;

namespace SkillCraft.Infrastructure.Queriers
{
  internal class AspectQuerier : IAspectQuerier
  {
    private readonly DbSet<Aspect> _aspects;

    public AspectQuerier(SkillCraftDbContext dbContext)
    {
      _aspects = dbContext.Aspects;
    }

    public async Task<Aspect?> GetAsync(Guid id, bool readOnly, CancellationToken cancellationToken)
    {
      return await _aspects.ApplyTracking(readOnly)
       .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedList<Aspect>> GetPagedAsync(int worldSid, string? search,
      AspectSort? sort, bool desc,
      int? index, int? count,
      bool readOnly, CancellationToken cancellationToken)
    {
      IQueryable<Aspect> query = _aspects.ApplyTracking(readOnly)
        .Where(x => x.WorldSid == worldSid);

      if (search != null)
      {
        foreach (string term in search.Split())
        {
          string pattern = $"%{term}%";

          query = query.Where(x => EF.Functions.ILike(x.Name, pattern));
        }
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          AspectSort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          AspectSort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The aspect sort '{sort}' is not valid.", nameof(sort)),
        };
      }

      query = query.ApplyPaging(index, count);

      Aspect[] aspects = await query.ToArrayAsync(cancellationToken);

      return new PagedList<Aspect>(aspects, total);
    }
  }
}
