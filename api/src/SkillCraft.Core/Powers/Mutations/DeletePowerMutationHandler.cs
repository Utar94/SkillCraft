using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Powers.Models;

namespace SkillCraft.Core.Powers.Mutations
{
  internal class DeletePowerMutationHandler : IRequestHandler<DeletePowerMutation, PowerModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public DeletePowerMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<PowerModel> Handle(DeletePowerMutation request, CancellationToken cancellationToken)
    {
      Power power = await _dbContext.Powers
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Power>(request.Id);

      if (power.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Power>(power, _appContext.UserId, _appContext.World);
      }

      power.Delete(_appContext.UserId);
      await _dbContext.SaveChangesAsync(cancellationToken);

      return _mapper.Map<PowerModel>(power);
    }
  }
}
