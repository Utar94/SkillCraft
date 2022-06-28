using Microsoft.EntityFrameworkCore;

namespace SkillCraft.Infrastructure.Extensions
{
  internal static class QueryableExtensions
  {
    public static IQueryable<T> ApplyTracking<T>(this IQueryable<T> query, bool readOnly)
      where T : class
    {
      ArgumentNullException.ThrowIfNull(query);

      return readOnly ? query.AsNoTracking() : query;
    }
  }
}
