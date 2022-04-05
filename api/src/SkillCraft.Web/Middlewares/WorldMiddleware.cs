using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using SkillCraft.Core.Worlds;
using SkillCraft.Infrastructure;

namespace SkillCraft.Web.Middlewares
{
  public class WorldMiddleware
  {
    private const string HeaderKey = "World";

    private readonly RequestDelegate next;

    public WorldMiddleware(RequestDelegate next)
    {
      this.next = next;
    }

    public async Task InvokeAsync(
      HttpContext httpContext,
      SkillCraftDbContext dbContext,
      IUserContext userContext
    )
    {
      if (httpContext.Request.Headers.TryGetValue(HeaderKey, out StringValues values))
      {
        string value = values.Single();
        World? world;
        if (Guid.TryParse(value, out Guid key))
        {
          world = await dbContext.Worlds.SingleOrDefaultAsync(x => x.Key == key);
        }
        else
        {
          string alias = value.ToLowerInvariant();
          world = await dbContext.Worlds.SingleOrDefaultAsync(x => x.Alias == alias);
        }

        if (world == null)
        {
          await SetResponseAsync(httpContext.Response, StatusCodes.Status404NotFound, HeaderKey);
          return;
        }
        else if (world.CreatedById != userContext.Id)
        {
          await SetResponseAsync(httpContext.Response, StatusCodes.Status403Forbidden);
          return;
        }

        if (!httpContext.SetWorld(world))
        {
          throw new InvalidOperationException("The World context item could not be set.");
        }
      }

      await next(httpContext);
    }

    private static async Task SetResponseAsync(
      HttpResponse response,
      int statusCode,
      string? field = null
    )
    {
      response.StatusCode = statusCode;

      if (field != null)
      {
        await response.WriteAsJsonAsync(new { field });
      }
    }
  }
}
