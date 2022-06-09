using SkillCraft.Core;
using SkillCraft.Core.Logging;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Web
{
  public class HttpApplicationContext : IApplicationContext
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpApplicationContext(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    protected HttpContext HttpContext => _httpContextAccessor.HttpContext
      ?? throw new InvalidOperationException($"The {nameof(_httpContextAccessor.HttpContext)} is required.");

    public EventLog EventLog => HttpContext.GetEventLog()
      ?? throw new InvalidOperationException($"The {nameof(EventLog)} context item is required.");
    public World World => HttpContext.GetWorld()
      ?? throw new InvalidOperationException($"The {nameof(World)} context item is required.");

    public void SetEntity(EntityBase entity)
    {
      ArgumentNullException.ThrowIfNull(entity);

      EventLog.EntityId = entity.Uuid;
      EventLog.EntityType = entity.GetType();
    }

    public bool TryGetWorld(out World? world)
    {
      world = HttpContext.GetWorld();

      return world != null;
    }
  }
}
