using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Worlds.Models;

namespace SkillCraft.Core.Worlds.Queries
{
  internal class GetWorldQueryHandler : IRequestHandler<GetWorldQuery, WorldModel?>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetWorldQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<WorldModel?> Handle(GetWorldQuery request, CancellationToken cancellationToken)
    {
      IQueryable<World> query = _dbContext.Worlds.AsNoTracking();
      World? world = null;
      if (Guid.TryParse(request.Id, out Guid uuid))
      {
        world = await query.SingleOrDefaultAsync(x => x.Uuid == uuid, cancellationToken);
      }
      else
      {
        string alias = request.Id.Trim().ToLowerInvariant();
        world = await _dbContext.Worlds.SingleOrDefaultAsync(x => x.Alias == alias, cancellationToken);
      }

      if (world == null)
      {
        return null;
      }
      else if (world.CreatedById != _appContext.UserId)
      {
        throw new UnauthorizedOperationException<World>(world, _appContext.UserId);
      }

      return _mapper.Map<WorldModel>(world);
    }
  }
}
