using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Conditions.Models;

namespace SkillCraft.Core.Conditions.Mutations
{
  internal class DeleteConditionMutationHandler : IRequestHandler<DeleteConditionMutation, ConditionModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public DeleteConditionMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<ConditionModel> Handle(DeleteConditionMutation request, CancellationToken cancellationToken)
    {
      Condition condition = await _dbContext.Conditions
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Condition>(request.Id);

      if (condition.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Condition>(condition, _appContext.UserId, _appContext.World);
      }

      condition.Delete(_appContext.UserId);
      await _dbContext.SaveChangesAsync(cancellationToken);

      return _mapper.Map<ConditionModel>(condition);
    }
  }
}
