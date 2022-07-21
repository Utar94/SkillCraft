using Microsoft.EntityFrameworkCore;
using SkillCraft.Core;
using SkillCraft.Core.Customizations;

namespace SkillCraft.Infrastructure.Queriers
{
  internal class CustomizationQuerier : ICustomizationQuerier
  {
    private readonly DbSet<Customization> _customizations;

    public CustomizationQuerier(SkillCraftDbContext dbContext)
    {
      _customizations = dbContext.Customizations;
    }

    public async Task<Customization?> GetAsync(Guid id, bool readOnly, CancellationToken cancellationToken)
    {
      return await _customizations.ApplyTracking(readOnly)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedList<Customization>> GetPagedAsync(int worldSid, string? search, CustomizationType? type,
      CustomizationSort? sort, bool desc,
      int? index, int? count,
      bool readOnly, CancellationToken cancellationToken)
    {
      IQueryable<Customization> query = _customizations.ApplyTracking(readOnly)
        .Where(x => x.WorldSid == worldSid);

      if (search != null)
      {
        foreach (string term in search.Split())
        {
          string pattern = $"%{term}%";

          query = query.Where(x => EF.Functions.ILike(x.Name, pattern));
        }
      }
      if (type.HasValue)
      {
        query = query.Where(x => x.Type == type.Value);
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          CustomizationSort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          CustomizationSort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The customization sort '{sort}' is not valid.", nameof(sort)),
        };
      }

      query = query.ApplyPaging(index, count);

      Customization[] customizations = await query.ToArrayAsync(cancellationToken);

      return new PagedList<Customization>(customizations, total);
    }
  }
}
