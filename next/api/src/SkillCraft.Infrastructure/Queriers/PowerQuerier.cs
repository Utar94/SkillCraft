using Microsoft.EntityFrameworkCore;
using SkillCraft.Core;
using SkillCraft.Core.Powers;

namespace SkillCraft.Infrastructure.Queriers
{
  internal class PowerQuerier : IPowerQuerier
  {
    private readonly DbSet<Power> _powers;

    public PowerQuerier(SkillCraftDbContext dbContext)
    {
      _powers = dbContext.Powers;
    }

    public async Task<Power?> GetAsync(Guid id, bool readOnly, CancellationToken cancellationToken)
    {
      return await _powers.ApplyTracking(readOnly)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedList<Power>> GetPagedAsync(int worldSid, string? search, IEnumerable<int>? tiers,
      PowerSort? sort, bool desc,
      int? index, int? count,
      bool readOnly, CancellationToken cancellationToken)
    {
      IQueryable<Power> query = _powers.ApplyTracking(readOnly)
        .Where(x => x.WorldSid == worldSid);

      if (search != null)
      {
        foreach (string term in search.Split())
        {
          string pattern = $"%{term}%";

          query = query.Where(x => EF.Functions.ILike(x.Name, pattern));
        }
      }
      if (tiers != null)
      {
        query = query.Where(x => tiers.Contains(x.Tier));
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          PowerSort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          PowerSort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The power sort '{sort}' is not valid.", nameof(sort)),
        };
      }

      query = query.ApplyPaging(index, count);

      Power[] powers = await query.ToArrayAsync(cancellationToken);

      return new PagedList<Power>(powers, total);
    }
  }
}
