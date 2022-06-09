using Logitar.Identity.Core;
using Logitar.WebApiToolKit.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core;
using SkillCraft.Core.Logging;
using SkillCraft.Core.Worlds;
using System.Text.Json;

namespace SkillCraft.Web.Pipeline
{
  public class HttpRequestPipeline : IRequestPipeline
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;

    public HttpRequestPipeline(
      IApplicationContext appContext,
      IDbContext dbContext,
      IMediator mediator,
      IUserContext userContext
    )
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mediator = mediator;
      _userContext = userContext;
    }

    public async Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
      ArgumentNullException.ThrowIfNull(request);

      EventLog eventLog = _appContext.EventLog;

      eventLog.CreatedById = _userContext.Id;
      eventLog.Data = JsonSerializer.Serialize<object>(request);
      eventLog.Name = request.GetType().Name;

      if (_appContext.TryGetWorld(out World? world) && world != null)
      {
        eventLog.WorldId = world.Uuid;
      }

      _dbContext.EventLogs.Add(eventLog);
      await _dbContext.SaveChangesAsync(cancellationToken);

      try
      {
        return await _mediator.Send(request, cancellationToken);
      }
      catch (ApiException exception)
      {
        var exceptionData = new ExceptionData(exception);
        eventLog.Errors.Add(TypedError<ExceptionData>.Failure(message: exception.Message, data: exceptionData, value: exceptionData.Value));

        throw;
      }
      catch (ErrorException exception)
      {
        eventLog.Errors.Add(exception.Error);

        throw;
      }
      catch (Exception exception)
      {
        if (exception is DbUpdateException)
        {
          _dbContext.CancelChanges();
        }

        var exceptionData = new ExceptionData(exception);
        eventLog.Errors.Add(TypedError<ExceptionData>.Failure(message: exception.Message, data: exceptionData));

        throw;
      }
    }
  }
}
