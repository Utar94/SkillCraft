using Microsoft.AspNetCore.Http.Extensions;
using SkillCraft.Core;
using SkillCraft.Core.Logging;

namespace SkillCraft.Web.Middlewares
{
  public class LoggingMiddleware
  {
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
      finally
      {
        if (eventLog.Name != null)
        {
          eventLog.Complete(httpContext.Response.StatusCode);
          await dbContext.SaveChangesAsync();
        }
      }
    }
  }
}
