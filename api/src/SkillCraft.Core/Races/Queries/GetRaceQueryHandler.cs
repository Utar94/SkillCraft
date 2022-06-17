using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Races.Models;

namespace SkillCraft.Core.Races.Queries
{
  internal class GetRaceQueryHandler : IRequestHandler<GetRaceQuery, RaceModel?>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetRaceQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<RaceModel?> Handle(GetRaceQuery request, CancellationToken cancellationToken)
    {
      Race? race = await _dbContext.Races
        .AsNoTracking()
        .Include(x => x.Languages)
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken);

      if (race == null)
      {
        return null;
      }
      else if (race.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Race>(race, _appContext.UserId, _appContext.World);
      }

      return _mapper.Map<RaceModel>(race);
    }
  }
}
