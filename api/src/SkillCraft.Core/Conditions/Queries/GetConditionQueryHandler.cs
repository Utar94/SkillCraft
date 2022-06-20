using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Conditions.Models;

namespace SkillCraft.Core.Conditions.Queries
{
  internal class GetConditionQueryHandler : IRequestHandler<GetConditionQuery, ConditionModel?>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetConditionQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<ConditionModel?> Handle(GetConditionQuery request, CancellationToken cancellationToken)
    {
      Condition? condition = await _dbContext.Conditions
        .AsNoTracking()
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken);

      if (condition == null)
      {
        return null;
      }
      else if (condition.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Condition>(condition, _appContext.UserId, _appContext.World);
      }

      return _mapper.Map<ConditionModel>(condition);
    }
  }
}
