using SkillCraft.Core.Logging;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Web
{
  public static class HttpContextExtensions
  {
    private const string EventLogKey = nameof(EventLog);
    private const string WorldKey = nameof(World);

    public static EventLog? GetEventLog(this HttpContext httpContext)
      => httpContext.GetItem<EventLog>(EventLogKey);
    public static World? GetWorld(this HttpContext httpContext)
      => httpContext.GetItem<World>(WorldKey);
    private static T? GetItem<T>(this HttpContext httpContext, object key)
    {
      if (httpContext.Items.TryGetValue(key, out object? value))
      {
        return (T?)value;
      }

      return default;
    }

    public static bool SetEventLog(this HttpContext httpContext, EventLog? eventLog)
      => httpContext.SetItem(EventLogKey, eventLog);
    public static bool SetWorld(this HttpContext httpContext, World? world)
      => httpContext.SetItem(WorldKey, world);
    private static bool SetItem<T>(this HttpContext httpContext, object key, T? value)
    {
      if (httpContext.Items.ContainsKey(key))
      {
        if (!httpContext.Items.Remove(key))
        {
          return false;
        }
      }

      return value != null && httpContext.Items.TryAdd(key, value);
    }
  }
}
