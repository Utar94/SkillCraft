using SkillCraft.Core;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Web
{
  internal class HttpUserContext : IUserContext
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpUserContext(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    protected HttpContext HttpContext => _httpContextAccessor.HttpContext
      ?? throw new InvalidOperationException($"The {nameof(_httpContextAccessor.HttpContext)} is required.");

    public Guid Id => Guid.Empty; // TODO(fpion): Authentication

    public World World => HttpContext.GetWorld()
      ?? throw new InvalidOperationException("The World is required.");
  }
}
