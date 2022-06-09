using AutoMapper;
using Logitar.Identity.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Worlds.Models;

namespace SkillCraft.Core.Worlds.Mutations
{
  internal class UpdateWorldMutationHandler : SaveWorldHandler, IRequestHandler<UpdateWorldMutation, WorldModel>
  {
    private readonly IDbContext _dbContext;
    private readonly IUserContext _userContext;

    public UpdateWorldMutationHandler(IDbContext dbContext, IMapper mapper, IUserContext userContext)
      : base(dbContext, mapper)
    {
      _dbContext = dbContext;
      _userContext = userContext;
    }

    public async Task<WorldModel> Handle(UpdateWorldMutation request, CancellationToken cancellationToken)
    {
      World world = await _dbContext.Worlds
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<World>(request.Id);

      if (world.CreatedById != _userContext.Id)
      {
        throw new UnauthorizedOperationException<World>(world, _userContext.Id);
      }

      world.Update(_userContext.Id);

      return await ExecuteAsync(world, request.Payload, cancellationToken);
    }
  }
}
