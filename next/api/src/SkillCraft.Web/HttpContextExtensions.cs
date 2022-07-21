using SkillCraft.Core.Worlds;

namespace SkillCraft.Web
{
  internal static class HttpContextExtensions
  {
    private const string WorldKey = nameof(World);

    public static World? GetWorld(this HttpContext context) => context.GetItem<World>(WorldKey);
    private static T? GetItem<T>(this HttpContext context, object key)
    {
      if (context.Items.TryGetValue(key, out object? value))
      {
        return (T?)value;
      }

      return default;
    }

    public static bool SetWorld(this HttpContext context, World? world) => context.SetItem(WorldKey, world);
    private static bool SetItem<T>(this HttpContext context, object key, T? value)
    {
      if (context.Items.ContainsKey(key))
      {
        if (!context.Items.Remove(key))
        {
          return false;
        }
      }

      return value != null && context.Items.TryAdd(key, value);
    }
  }
}
