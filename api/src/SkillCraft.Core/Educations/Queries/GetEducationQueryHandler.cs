using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Educations.Models;

namespace SkillCraft.Core.Educations.Queries
{
  internal class GetEducationQueryHandler : IRequestHandler<GetEducationQuery, EducationModel?>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetEducationQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<EducationModel?> Handle(GetEducationQuery request, CancellationToken cancellationToken)
    {
      Education? education = await _dbContext.Educations
        .AsNoTracking()
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken);

      if (education == null)
      {
        return null;
      }
      else if (education.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Education>(education, _appContext.UserId, _appContext.World);
      }

      return _mapper.Map<EducationModel>(education);
    }
  }
}
