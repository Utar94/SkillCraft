using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Languages.Models;

namespace SkillCraft.Core.Languages.Queries
{
  internal class GetLanguageQueryHandler : IRequestHandler<GetLanguageQuery, LanguageModel?>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetLanguageQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<LanguageModel?> Handle(GetLanguageQuery request, CancellationToken cancellationToken)
    {
      Language? language = await _dbContext.Languages
        .AsNoTracking()
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken);

      if (language == null)
      {
        return null;
      }
      else if (language.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Language>(language, _appContext.UserId, _appContext.World);
      }

      return _mapper.Map<LanguageModel>(language);
    }
  }
}
