using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Powers.Models;

namespace SkillCraft.Core.Powers.Mutations
{
  internal class UpdatePowerMutationHandler : SavePowerHandler, IRequestHandler<UpdatePowerMutation, PowerModel>
  {
    public UpdatePowerMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<PowerModel> Handle(UpdatePowerMutation request, CancellationToken cancellationToken)
    {
      Power power = await DbContext.Powers
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Power>(request.Id);

      if (power.WorldId != AppContext.World.Id)
      {
        throw new UnauthorizedOperationException<Power>(power, AppContext.UserId, AppContext.World);
      }

      power.Update(AppContext.UserId);

      return await ExecuteAsync(power, request.Payload, cancellationToken);
    }
  }
}
