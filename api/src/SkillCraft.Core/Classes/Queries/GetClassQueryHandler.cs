using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Classes.Models;

namespace SkillCraft.Core.Classes.Queries
{
  internal class GetClassQueryHandler : IRequestHandler<GetClassQuery, ClassModel?>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetClassQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<ClassModel?> Handle(GetClassQuery request, CancellationToken cancellationToken)
    {
      Class? talent = await _dbContext.Classes
        .AsNoTracking()
        .Include(x => x.Talents).ThenInclude(x => x.Talent)
        .Include(x => x.UniqueTalent)
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken);

      if (talent == null)
      {
        return null;
      }
      else if (talent.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Class>(talent, _appContext.UserId, _appContext.World);
      }

      return _mapper.Map<ClassModel>(talent);
    }
  }
}
