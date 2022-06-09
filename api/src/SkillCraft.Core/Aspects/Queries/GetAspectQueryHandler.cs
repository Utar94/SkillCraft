using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Aspects.Models;

namespace SkillCraft.Core.Aspects.Queries
{
  internal class GetAspectQueryHandler : IRequestHandler<GetAspectQuery, AspectModel?>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAspectQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<AspectModel?> Handle(GetAspectQuery request, CancellationToken cancellationToken)
    {
      Aspect? aspect = await _dbContext.Aspects
        .AsNoTracking()
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken);

      if (aspect == null)
      {
        return null;
      }
      else if (aspect.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Aspect>(aspect, _appContext.UserId, _appContext.World);
      }

      return _mapper.Map<AspectModel>(aspect);
    }
  }
}
