using Logitar.Identity.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using SkillCraft.Core;
using SkillCraft.Core.Logging;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Web.Middlewares
{
  public class WorldMiddleware
  {
    public const string HeaderKey = nameof(World);

    private readonly RequestDelegate _next;

    public WorldMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, IDbContext dbContext, IUserContext userContext)
    {
      if (httpContext.Request.Headers.TryGetValue(HeaderKey, out StringValues values))
      {
        string value = values.Single();

        World? world;
        if (Guid.TryParse(value, out Guid uuid))
        {
          world = await dbContext.Worlds.SingleOrDefaultAsync(x => x.Uuid == uuid);
        }
        else
        {
          string alias = value.Trim().ToLowerInvariant();
          world = await dbContext.Worlds.SingleOrDefaultAsync(x => x.Alias == alias);
        }

        if (world == null)
        {
          throw Error.FailureException(ErrorCode.WorldNotFound, $"The world \"{value}\" could not be found.");
        }
        else if (world.CreatedById != userContext.Id)
        {
          throw Error.FailureException(ErrorCode.WorldForbidden, "Access denied.");
        }
        else if (!httpContext.SetWorld(world))
        {
          throw new InvalidOperationException("The world context item could not be set.");
        }
      }

      await _next(httpContext);
    }
  }
}
