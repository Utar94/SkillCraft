using Microsoft.AspNetCore.Http.Extensions;
using SkillCraft.Core;
using SkillCraft.Core.Logging;

namespace SkillCraft.Web.Middlewares
{
  public class LoggingMiddleware
  {
    private static readonly Dictionary<ErrorCode, ErrorHandler> _handlers = new()
    {
      [ErrorCode.WorldForbidden] = new(StatusCodes.Status403Forbidden, new { code = nameof(ErrorCode.WorldForbidden) }),
      [ErrorCode.WorldNotFound] = new(StatusCodes.Status404NotFound, new { code = nameof(ErrorCode.WorldNotFound) }),
      [ErrorCode.WorldRequired] = new(StatusCodes.Status401Unauthorized, new { code = nameof(ErrorCode.WorldRequired) })
    };

    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, IDbContext dbContext)
    {
      var eventLog = new EventLog(Guid.Empty)
      {
        Method = httpContext.Request.Method,
        Url = httpContext.Request.GetDisplayUrl()
      };
      if (!httpContext.SetEventLog(eventLog))
      {
        throw new InvalidOperationException("The event log context item could not be set.");
      }

      try
      {
        await _next(httpContext);
      }
      catch (ErrorException exception)
      {
        if (exception.Error.Code.HasValue && _handlers.TryGetValue(exception.Error.Code.Value, out ErrorHandler handler))
        {
          httpContext.Response.StatusCode = handler.StatusCode;

          if (handler.Value != null)
          {
            await httpContext.Response.WriteAsJsonAsync(handler.Value);
          }
        }
        else
        {
          throw;
        }
      }
      finally
      {
        if (eventLog.HasErrors)
        {
          dbContext.CancelChanges();
        }

        if (eventLog.Name != null)
        {
          eventLog.Complete(httpContext.Response.StatusCode);
          await dbContext.SaveChangesAsync();
        }
      }
    }
  }
}
