using Microsoft.Extensions.Primitives;
using SkillCraft.Core;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Web.Middlewares
{
  internal class WorldMiddleware
  {
    private readonly RequestDelegate _next;

    public WorldMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, IUserContext userContext, IWorldQuerier worldQuerier)
    {
      if (httpContext.Request.Headers.TryGetValue(Constants.Headers.World, out StringValues values))
      {
        string alias = values.Single();
        World? world = Guid.TryParse(alias, out Guid id)
          ? await worldQuerier.GetAsync(id, readOnly: false)
          : await worldQuerier.GetAsync(alias, readOnly: false);

        if (world == null)
        {
          httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
          await httpContext.Response.WriteAsJsonAsync(new { code = "WorldNotFound" });
          return;
        }
        else if (world.CreatedById != userContext.Id)
        {
          httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
          return;
        }

        if (!httpContext.SetWorld(world))
        {
          throw new InvalidOperationException("The World context item could not be set.");
        }
      }

      await _next(httpContext);
    }
  }
}
