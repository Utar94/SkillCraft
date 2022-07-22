using Microsoft.EntityFrameworkCore;
using SkillCraft.Core;
using SkillCraft.Core.Talents;

namespace SkillCraft.Infrastructure.Queriers
{
  internal class TalentQuerier : ITalentQuerier
  {
    private readonly DbSet<Talent> _talents;

    public TalentQuerier(SkillCraftDbContext dbContext)
    {
      _talents = dbContext.Talents;
    }

    public async Task<Talent?> GetAsync(Guid id, bool readOnly, CancellationToken cancellationToken)
    {
      return await _talents.ApplyTracking(readOnly)
        .Include(x => x.Options)
        .Include(x => x.RequiredTalent)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedList<Talent>> GetPagedAsync(int worldSid, bool? multipleAcquisition, string? search, Skill? skill, IEnumerable<int>? tiers,
      TalentSort? sort, bool desc,
      int? index, int? count,
      bool readOnly, CancellationToken cancellationToken)
    {
      IQueryable<Talent> query = _talents.ApplyTracking(readOnly)
        .Include(x => x.RequiredTalent)
        .Where(x => x.WorldSid == worldSid);

      if (multipleAcquisition.HasValue)
      {
        query = query.Where(x => x.MultipleAcquisition == multipleAcquisition.Value);
      }
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
      if (tiers != null)
      {
        query = query.Where(x => tiers.Contains(x.Tier));
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          TalentSort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          TalentSort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The talent sort '{sort}' is not valid.", nameof(sort)),
        };
      }

      query = query.ApplyPaging(index, count);

      Talent[] talents = await query.ToArrayAsync(cancellationToken);

      return new PagedList<Talent>(talents, total);
    }
  }
}
