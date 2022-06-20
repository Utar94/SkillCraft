using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Races.Models;

namespace SkillCraft.Core.Races.Mutations
{
  internal class UpdateRaceMutationHandler : SaveRaceHandler, IRequestHandler<UpdateRaceMutation, RaceModel>
  {
    public UpdateRaceMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<RaceModel> Handle(UpdateRaceMutation request, CancellationToken cancellationToken)
    {
      Race nature = await DbContext.Races
        .Include(x => x.Languages)
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Race>(request.Id);

      if (nature.WorldId != AppContext.World.Id)
      {
        throw new UnauthorizedOperationException<Race>(nature, AppContext.UserId, AppContext.World);
      }

      nature.Update(AppContext.UserId);

      return await ExecuteAsync(nature, request.Payload, cancellationToken);
    }
  }
}
