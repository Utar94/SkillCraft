using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Talents.Models;

namespace SkillCraft.Core.Talents.Queries
{
  internal class GetTalentQueryHandler : IRequestHandler<GetTalentQuery, TalentModel?>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetTalentQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<TalentModel?> Handle(GetTalentQuery request, CancellationToken cancellationToken)
    {
      Talent? talent = await _dbContext.Talents
        .AsNoTracking()
        .Include(x => x.Options)
        .Include(x => x.RequiredTalent)
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken);

      if (talent == null)
      {
        return null;
      }
      else if (talent.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Talent>(talent, _appContext.UserId, _appContext.World);
      }

      return _mapper.Map<TalentModel>(talent);
    }
  }
}
