using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Castes.Models;

namespace SkillCraft.Core.Castes.Queries
{
  internal class GetCasteQueryHandler : IRequestHandler<GetCasteQuery, CasteModel?>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetCasteQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<CasteModel?> Handle(GetCasteQuery request, CancellationToken cancellationToken)
    {
      Caste? caste = await _dbContext.Castes
        .AsNoTracking()
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken);

      if (caste == null)
      {
        return null;
      }
      else if (caste.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Caste>(caste, _appContext.UserId, _appContext.World);
      }

      return _mapper.Map<CasteModel>(caste);
    }
  }
}
