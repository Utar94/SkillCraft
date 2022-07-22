using Microsoft.EntityFrameworkCore;
using SkillCraft.Core;
using SkillCraft.Core.Races;

namespace SkillCraft.Infrastructure.Queriers
{
  internal class RaceQuerier : IRaceQuerier
  {
    private readonly DbSet<Race> _races;

    public RaceQuerier(SkillCraftDbContext dbContext)
    {
      _races = dbContext.Races;
    }

    public async Task<Race?> GetAsync(Guid id, bool readOnly, CancellationToken cancellationToken)
    {
      return await _races.ApplyTracking(readOnly)
        .Include(x => x.Languages)
        .Include(x => x.Parent)
        .Include(x => x.Traits)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedList<Race>> GetPagedAsync(int worldSid, Guid? parentId, string? search, SizeCategory? size,
      RaceSort? sort, bool desc,
      int? index, int? count,
      bool readOnly, CancellationToken cancellationToken)
    {
      IQueryable<Race> query = _races.ApplyTracking(readOnly)
        .Include(x => x.Parent)
        .Where(x => x.WorldSid == worldSid);

      query = parentId.HasValue
        ? query.Where(x => x.Parent != null && x.Parent.Id == parentId.Value)
        : query.Where(x => x.Parent == null);

      if (search != null)
      {
        foreach (string term in search.Split())
        {
          string pattern = $"%{term}%";

          query = query.Where(x => EF.Functions.ILike(x.Name, pattern));
        }
      }
      if (size.HasValue)
      {
        query = query.Where(x => x.Size == size.Value);
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          RaceSort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          RaceSort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The race sort '{sort}' is not valid.", nameof(sort)),
        };
      }

      query = query.ApplyPaging(index, count);

      Race[] races = await query.ToArrayAsync(cancellationToken);

      return new PagedList<Race>(races, total);
    }
  }
}
