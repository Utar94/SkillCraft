using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Races.Models;

namespace SkillCraft.Core.Races.Mutations
{
  internal class DeleteRaceMutationHandler : IRequestHandler<DeleteRaceMutation, RaceModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public DeleteRaceMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<RaceModel> Handle(DeleteRaceMutation request, CancellationToken cancellationToken)
    {
      Race race = await _dbContext.Races
        .Include(x => x.Languages)
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Race>(request.Id);

      if (race.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Race>(race, _appContext.UserId, _appContext.World);
      }

      race.Delete(_appContext.UserId);
      await _dbContext.SaveChangesAsync(cancellationToken);

      return _mapper.Map<RaceModel>(race);
    }
  }
}
