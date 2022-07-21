using Microsoft.EntityFrameworkCore;
using SkillCraft.Core;
using SkillCraft.Core.Educations;

namespace SkillCraft.Infrastructure.Queriers
{
  internal class EducationQuerier : IEducationQuerier
  {
    private readonly DbSet<Education> _educations;

    public EducationQuerier(SkillCraftDbContext dbContext)
    {
      _educations = dbContext.Educations;
    }

    public async Task<Education?> GetAsync(Guid id, bool readOnly, CancellationToken cancellationToken)
    {
      return await _educations.ApplyTracking(readOnly)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedList<Education>> GetPagedAsync(int worldSid, string? search, Skill? skill,
      EducationSort? sort, bool desc,
      int? index, int? count,
      bool readOnly, CancellationToken cancellationToken)
    {
      IQueryable<Education> query = _educations.ApplyTracking(readOnly)
        .Where(x => x.WorldSid == worldSid);

      if (skill.HasValue)
      {
        query = query.Where(x => x.Skill == skill.Value);
      }
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
          EducationSort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          EducationSort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The education sort '{sort}' is not valid.", nameof(sort)),
        };
      }

      query = query.ApplyPaging(index, count);

      Education[] educations = await query.ToArrayAsync(cancellationToken);

      return new PagedList<Education>(educations, total);
    }
  }
}
