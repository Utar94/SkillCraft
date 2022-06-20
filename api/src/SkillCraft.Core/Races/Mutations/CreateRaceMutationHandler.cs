using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Races.Models;

namespace SkillCraft.Core.Races.Mutations
{
  internal class CreateRaceMutationHandler : SaveRaceHandler, IRequestHandler<CreateRaceMutation, RaceModel>
  {
    public CreateRaceMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<RaceModel> Handle(CreateRaceMutation request, CancellationToken cancellationToken)
    {
      Race? parent = null;
      if (request.Payload.ParentId.HasValue)
      {
        parent = await DbContext.Races
          .SingleOrDefaultAsync(x => x.Uuid == request.Payload.ParentId.Value, cancellationToken)
          ?? throw new EntityNotFoundException<Race>(request.Payload.ParentId.Value, nameof(request.Payload.ParentId));

        if (parent.WorldId != AppContext.World.Id)
        {
          throw new UnauthorizedOperationException<Race>(parent, AppContext.UserId, AppContext.World);
        }
        else if (parent.ParentId.HasValue)
        {
          throw new InvalidParentRaceException(request.Payload.ParentId.Value);
        }
      }

      var race = new Race(AppContext.UserId, AppContext.World, parent);

      DbContext.Races.Add(race);

      return await ExecuteAsync(race, request.Payload, cancellationToken);
    }
  }
}
