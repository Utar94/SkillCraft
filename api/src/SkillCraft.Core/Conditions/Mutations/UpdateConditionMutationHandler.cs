using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Conditions.Models;

namespace SkillCraft.Core.Conditions.Mutations
{
  internal class UpdateConditionMutationHandler : SaveConditionHandler, IRequestHandler<UpdateConditionMutation, ConditionModel>
  {
    public UpdateConditionMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<ConditionModel> Handle(UpdateConditionMutation request, CancellationToken cancellationToken)
    {
      Condition condition = await DbContext.Conditions
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Condition>(request.Id);

      if (condition.WorldId != AppContext.World.Id)
      {
        throw new UnauthorizedOperationException<Condition>(condition, AppContext.UserId, AppContext.World);
      }

      condition.Update(AppContext.UserId);

      return await ExecuteAsync(condition, request.Payload, cancellationToken);
    }
  }
}
