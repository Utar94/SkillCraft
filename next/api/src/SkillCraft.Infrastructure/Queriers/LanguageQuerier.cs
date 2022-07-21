using Microsoft.EntityFrameworkCore;
using SkillCraft.Core;
using SkillCraft.Core.Languages;

namespace SkillCraft.Infrastructure.Queriers
{
  internal class LanguageQuerier : ILanguageQuerier
  {
    private readonly DbSet<Language> _languages;

    public LanguageQuerier(SkillCraftDbContext dbContext)
    {
      _languages = dbContext.Languages;
    }

    public async Task<Language?> GetAsync(Guid id, bool readOnly, CancellationToken cancellationToken)
    {
      return await _languages.ApplyTracking(readOnly)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedList<Language>> GetPagedAsync(int worldSid, bool? isExotic, string? search,
      LanguageSort? sort, bool desc,
      int? index, int? count,
      bool readOnly, CancellationToken cancellationToken)
    {
      IQueryable<Language> query = _languages.ApplyTracking(readOnly)
        .Where(x => x.WorldSid == worldSid);

      if (isExotic.HasValue)
      {
        query = query.Where(x => x.IsExotic == isExotic.Value);
      }
      if (search != null)
      {
        foreach (string term in search.Split())
        {
          string pattern = $"%{term}%";

          query = query.Where(x => EF.Functions.ILike(x.Name, pattern)
            || (x.Script != null && EF.Functions.ILike(x.Script, pattern))
            || (x.TypicalSpeakers != null && EF.Functions.ILike(x.TypicalSpeakers, pattern)));
        }
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          LanguageSort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          LanguageSort.Script => desc ? query.OrderByDescending(x => x.Script).ThenBy(x => x.Name) : query.OrderBy(x => x.Script).ThenBy(x => x.Name),
          LanguageSort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The language sort '{sort}' is not valid.", nameof(sort)),
        };
      }

      query = query.ApplyPaging(index, count);

      Language[] languages = await query.ToArrayAsync(cancellationToken);

      return new PagedList<Language>(languages, total);
    }
  }
}
