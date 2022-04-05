using SkillCraft.Core.Worlds;

namespace SkillCraft.Web
{
  public static class HttpContextExtensions
  {
    private const string WorldKey = nameof(World);

    public static World? GetWorld(this HttpContext context)
    {
      if (context.Items.TryGetValue(WorldKey, out object? value))
      {
        return (World?)value;
      }

      return null;
    }

    public static bool SetWorld(this HttpContext context, World? world)
    {
      if (context.Items.ContainsKey(WorldKey))
      {
        if (!context.Items.Remove(WorldKey))
        {
          return false;
        }
      }

      return world != null && context.Items.TryAdd(WorldKey, world);
    }
  }
}
