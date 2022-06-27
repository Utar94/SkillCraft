using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Powers.Models;

namespace SkillCraft.Core.Powers.Queries
{
  internal class GetPowerQueryHandler : IRequestHandler<GetPowerQuery, PowerModel?>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetPowerQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<PowerModel?> Handle(GetPowerQuery request, CancellationToken cancellationToken)
    {
      Power? power = await _dbContext.Powers
        .AsNoTracking()
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken);

      if (power == null)
      {
        return null;
      }
      else if (power.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Power>(power, _appContext.UserId, _appContext.World);
      }

      return _mapper.Map<PowerModel>(power);
    }
  }
}
