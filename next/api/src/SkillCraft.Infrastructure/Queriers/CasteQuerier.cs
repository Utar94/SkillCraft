using Microsoft.EntityFrameworkCore;
using SkillCraft.Core;
using SkillCraft.Core.Castes;

namespace SkillCraft.Infrastructure.Queriers
{
  internal class CasteQuerier : ICasteQuerier
  {
    private readonly DbSet<Caste> _castes;

    public CasteQuerier(SkillCraftDbContext dbContext)
    {
      _castes = dbContext.Castes;
    }

    public async Task<Caste?> GetAsync(Guid id, bool readOnly, CancellationToken cancellationToken)
    {
      return await _castes.ApplyTracking(readOnly)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedList<Caste>> GetPagedAsync(int worldSid, string? search, Skill? skill,
      CasteSort? sort, bool desc,
      int? index, int? count,
      bool readOnly, CancellationToken cancellationToken)
    {
      IQueryable<Caste> query = _castes.ApplyTracking(readOnly)
        .Where(x => x.WorldSid == worldSid);

      if (search != null)
      {
        foreach (string term in search.Split())
        {
          string pattern = $"%{term}%";

          query = query.Where(x => EF.Functions.ILike(x.Name, pattern));
        }
      }
      if (skill.HasValue)
      {
        query = query.Where(x => x.Skill == skill.Value);
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          CasteSort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          CasteSort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The caste sort '{sort}' is not valid.", nameof(sort)),
        };
      }

      query = query.ApplyPaging(index, count);

      Caste[] castes = await query.ToArrayAsync(cancellationToken);

      return new PagedList<Caste>(castes, total);
    }
  }
}
