using SkillCraft.Core.Worlds;

namespace SkillCraft.Web
{
  public class HttpUserContext : Logitar.AspNetCore.Identity.HttpUserContext, IUserContext
  {
    public HttpUserContext(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
    }

    public World World => HttpContext.GetWorld()
      ?? throw new InvalidOperationException("The World context item is required.");
  }
}
