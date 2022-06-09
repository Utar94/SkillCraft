using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Natures.Models;

namespace SkillCraft.Core.Natures.Queries
{
  internal class GetNatureQueryHandler : IRequestHandler<GetNatureQuery, NatureModel?>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetNatureQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<NatureModel?> Handle(GetNatureQuery request, CancellationToken cancellationToken)
    {
      Nature? nature = await _dbContext.Natures
        .AsNoTracking()
        .Include(x => x.Feat)
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken);

      if (nature == null)
      {
        return null;
      }
      else if (nature.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Nature>(nature, _appContext.UserId, _appContext.World);
      }

      return _mapper.Map<NatureModel>(nature);
    }
  }
}
