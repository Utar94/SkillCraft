using AutoMapper;
using Logitar.Identity.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Worlds.Models;

namespace SkillCraft.Core.Worlds.Mutations
{
  internal class DeleteWorldMutationHandler : IRequestHandler<DeleteWorldMutation, WorldModel>
  {
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public DeleteWorldMutationHandler(IDbContext dbContext, IMapper mapper, IUserContext userContext)
    {
      _dbContext = dbContext;
      _mapper = mapper;
      _userContext = userContext;
    }

    public async Task<WorldModel> Handle(DeleteWorldMutation request, CancellationToken cancellationToken)
    {
      World world = await _dbContext.Worlds
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<World>(request.Id);

      if (world.CreatedById != _userContext.Id)
      {
        throw new UnauthorizedOperationException<World>(world, _userContext.Id);
      }

      _dbContext.Worlds.Remove(world);
      await _dbContext.SaveChangesAsync(cancellationToken);

      return _mapper.Map<WorldModel>(world);
    }
  }
}
