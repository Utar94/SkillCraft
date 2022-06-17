using AutoMapper;
using MediatR;
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
      var race = new Race(AppContext.UserId, AppContext.World);

      DbContext.Races.Add(race);

      return await ExecuteAsync(race, request.Payload, cancellationToken);
    }
  }
}
