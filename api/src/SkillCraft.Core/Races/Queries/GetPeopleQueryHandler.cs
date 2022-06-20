using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Races.Models;

namespace SkillCraft.Core.Races.Queries
{
  internal class GetPeopleQueryHandler : IRequestHandler<GetPeopleQuery, IEnumerable<RaceModel>>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetPeopleQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<IEnumerable<RaceModel>> Handle(GetPeopleQuery request, CancellationToken cancellationToken)
    {
      Race race = await _dbContext.Races
        .AsNoTracking()
        .Include(x => x.Children.Where(x => !x.Deleted))
        .SingleOrDefaultAsync(x => x.Uuid == request.RaceId, cancellationToken)
        ?? throw new EntityNotFoundException<Race>(request.RaceId);

      if (race.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Race>(race, _appContext.UserId, _appContext.World);
      }

      return _mapper.Map<IEnumerable<RaceModel>>(race.Children);
    }
  }
}
