using AutoMapper;
using MediatR;
using SkillCraft.Core.Conditions.Models;

namespace SkillCraft.Core.Conditions.Mutations
{
  internal class CreateConditionMutationHandler : SaveConditionHandler, IRequestHandler<CreateConditionMutation, ConditionModel>
  {
    public CreateConditionMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<ConditionModel> Handle(CreateConditionMutation request, CancellationToken cancellationToken)
    {
      var condition = new Condition(AppContext.UserId, AppContext.World);

      DbContext.Conditions.Add(condition);

      return await ExecuteAsync(condition, request.Payload, cancellationToken);
    }
  }
}
