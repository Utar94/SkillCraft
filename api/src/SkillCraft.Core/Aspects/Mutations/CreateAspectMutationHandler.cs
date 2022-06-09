using AutoMapper;
using MediatR;
using SkillCraft.Core.Aspects.Models;

namespace SkillCraft.Core.Aspects.Mutations
{
  internal class CreateAspectMutationHandler : SaveAspectHandler, IRequestHandler<CreateAspectMutation, AspectModel>
  {
    public CreateAspectMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<AspectModel> Handle(CreateAspectMutation request, CancellationToken cancellationToken)
    {
      var aspect = new Aspect(AppContext.UserId, AppContext.World);

      DbContext.Aspects.Add(aspect);

      return await ExecuteAsync(aspect, request.Payload, cancellationToken);
    }
  }
}
