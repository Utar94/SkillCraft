using Microsoft.EntityFrameworkCore;
using SkillCraft.Core;
using SkillCraft.Core.Natures;

namespace SkillCraft.Infrastructure.Queriers
{
  internal class NatureQuerier : INatureQuerier
  {
    private readonly DbSet<Nature> _natures;

    public NatureQuerier(SkillCraftDbContext dbContext)
    {
      _natures = dbContext.Natures;
    }

    public async Task<Nature?> GetAsync(Guid id, bool readOnly, CancellationToken cancellationToken)
    {
      return await _natures.ApplyTracking(readOnly)
        .Include(x => x.Feat)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedList<Nature>> GetPagedAsync(int worldSid, Core.Attribute? attribute, string? search,
      NatureSort? sort, bool desc,
      int? index, int? count,
      bool readOnly, CancellationToken cancellationToken)
    {
      IQueryable<Nature> query = _natures.ApplyTracking(readOnly)
        .Include(x => x.Feat)
        .Where(x => x.WorldSid == worldSid);

      if (attribute.HasValue)
      {
        query = query.Where(x => x.Attribute == attribute.Value);
      }
      if (search != null)
      {
        foreach (string term in search.Split())
        {
          string pattern = $"%{term}%";

          query = query.Where(x => EF.Functions.ILike(x.Name, pattern)
            || (x.Feat != null && EF.Functions.ILike(x.Feat.Name, pattern)));
        }
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          NatureSort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          NatureSort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The nature sort '{sort}' is not valid.", nameof(sort)),
        };
      }

      query = query.ApplyPaging(index, count);

      Nature[] natures = await query.ToArrayAsync(cancellationToken);

      return new PagedList<Nature>(natures, total);
    }
  }
}
