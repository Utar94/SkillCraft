using AutoMapper;
using MediatR;
using SkillCraft.Core.Powers.Models;

namespace SkillCraft.Core.Powers.Mutations
{
  internal class CreatePowerMutationHandler : SavePowerHandler, IRequestHandler<CreatePowerMutation, PowerModel>
  {
    public CreatePowerMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<PowerModel> Handle(CreatePowerMutation request, CancellationToken cancellationToken)
    {
      var power = new Power(request.Payload.Tier, AppContext.UserId, AppContext.World);

      DbContext.Powers.Add(power);

      return await ExecuteAsync(power, request.Payload, cancellationToken);
    }
  }
}
