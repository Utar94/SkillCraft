using MediatR;

namespace SkillCraft.Web.Pipeline
{
  public interface IRequestPipeline
  {
    Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
  }
}
