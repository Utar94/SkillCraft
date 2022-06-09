using AutoMapper;
using Logitar.Identity.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Worlds.Models;

namespace SkillCraft.Core.Worlds.Queries
{
  internal class GetWorldQueryHandler : IRequestHandler<GetWorldQuery, WorldModel?>
  {
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public GetWorldQueryHandler(IDbContext dbContext, IMapper mapper, IUserContext userContext)
    {
      _dbContext = dbContext;
      _mapper = mapper;
      _userContext = userContext;
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
      else if (world.CreatedById != _userContext.Id)
      {
        throw new UnauthorizedOperationException<World>(world, _userContext.Id);
      }

      return _mapper.Map<WorldModel>(world);
    }
  }
}
