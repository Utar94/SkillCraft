using Microsoft.EntityFrameworkCore;
using SkillCraft.Core;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Infrastructure.Queriers
{
  internal class WorldQuerier : IWorldQuerier
  {
    private readonly DbSet<World> _worlds;

    public WorldQuerier(SkillCraftDbContext dbContext)
    {
      _worlds = dbContext.Worlds;
    }

    public async Task<World?> GetAsync(string alias, bool readOnly, CancellationToken cancellationToken)
    {
      alias = alias?.ToUpper() ?? throw new ArgumentNullException(nameof(alias));

      return await _worlds.ApplyTracking(readOnly)
        .SingleOrDefaultAsync(x => x.AliasNormalized == alias, cancellationToken);
    }

    public async Task<World?> GetAsync(Guid id, bool readOnly, CancellationToken cancellationToken)
    {
      return await _worlds.ApplyTracking(readOnly)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedList<World>> GetPagedAsync(Guid userId, string? search,
      WorldSort? sort, bool desc,
      int? index, int? count,
      bool readOnly, CancellationToken cancellationToken)
    {
      IQueryable<World> query = _worlds.ApplyTracking(readOnly)
        .Where(x => x.CreatedById == userId);

      if (search != null)
      {
        foreach (string term in search.Split())
        {
          string pattern = $"%{term}%";

          query = query.Where(x => EF.Functions.ILike(x.Alias, pattern) || EF.Functions.ILike(x.Name, pattern));
        }
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          WorldSort.Alias => desc ? query.OrderByDescending(x => x.Alias) : query.OrderBy(x => x.Alias),
          WorldSort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          WorldSort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The world sort '{sort}' is not valid.", nameof(sort)),
        };
      }

      query = query.ApplyPaging(index, count);

      World[] worlds = await query.ToArrayAsync(cancellationToken);

      return new PagedList<World>(worlds, total);
    }
  }
}
