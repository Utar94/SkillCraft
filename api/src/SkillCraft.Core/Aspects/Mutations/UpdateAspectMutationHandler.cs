using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Aspects.Models;

namespace SkillCraft.Core.Aspects.Mutations
{
  internal class UpdateAspectMutationHandler : SaveAspectHandler, IRequestHandler<UpdateAspectMutation, AspectModel>
  {
    public UpdateAspectMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<AspectModel> Handle(UpdateAspectMutation request, CancellationToken cancellationToken)
    {
      Aspect aspect = await DbContext.Aspects
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Aspect>(request.Id);

      if (aspect.WorldId != AppContext.World.Id)
      {
        throw new UnauthorizedOperationException<Aspect>(aspect, AppContext.UserId, AppContext.World);
      }

      aspect.Update(AppContext.UserId);

      return await ExecuteAsync(aspect, request.Payload, cancellationToken);
    }
  }
}
