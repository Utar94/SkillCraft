using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Worlds.Models;

namespace SkillCraft.Core.Worlds.Mutations
{
  internal class UpdateWorldMutationHandler : SaveWorldHandler, IRequestHandler<UpdateWorldMutation, WorldModel>
  {
    public UpdateWorldMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<WorldModel> Handle(UpdateWorldMutation request, CancellationToken cancellationToken)
    {
      World world = await DbContext.Worlds
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<World>(request.Id);

      if (world.CreatedById != AppContext.UserId)
      {
        throw new UnauthorizedOperationException<World>(world, AppContext.UserId);
      }

      world.Update(AppContext.UserId);

      return await ExecuteAsync(world, request.Payload, cancellationToken);
    }
  }
}
