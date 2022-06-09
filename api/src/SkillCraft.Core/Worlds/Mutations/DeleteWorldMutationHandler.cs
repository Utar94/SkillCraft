using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Worlds.Models;

namespace SkillCraft.Core.Worlds.Mutations
{
  internal class DeleteWorldMutationHandler : IRequestHandler<DeleteWorldMutation, WorldModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public DeleteWorldMutationHandler(
      IApplicationContext appContext,
      IDbContext dbContext,
      IMapper mapper
    )
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<WorldModel> Handle(DeleteWorldMutation request, CancellationToken cancellationToken)
    {
      World world = await _dbContext.Worlds
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<World>(request.Id);

      if (world.CreatedById != _appContext.UserId)
      {
        throw new UnauthorizedOperationException<World>(world, _appContext.UserId);
      }

      _dbContext.Worlds.Remove(world);
      await _dbContext.SaveChangesAsync(cancellationToken);

      _appContext.SetEntity(world);

      return _mapper.Map<WorldModel>(world);
    }
  }
}
