using AutoMapper;

namespace SkillCraft.Core
{
  public class ListModel<T>
  {
    private ListModel(IEnumerable<T> items, long total)
    {
      Items = items ?? throw new ArgumentNullException(nameof(items));
      Total = total;
    }

    public IEnumerable<T> Items { get; private set; }
    public long Total { get; private set; }

    public static ListModel<T> From<TSource>(PagedList<TSource> collection, IMapper mapper)
    {
      return new ListModel<T>(mapper.Map<IEnumerable<T>>(collection), collection.Total);
    }
  }
}
