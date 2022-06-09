using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Customizations.Models;

namespace SkillCraft.Core.Customizations.Queries
{
  internal class GetCustomizationQueryHandler : IRequestHandler<GetCustomizationQuery, CustomizationModel?>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetCustomizationQueryHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<CustomizationModel?> Handle(GetCustomizationQuery request, CancellationToken cancellationToken)
    {
      Customization? customization = await _dbContext.Customizations
        .AsNoTracking()
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken);

      if (customization == null)
      {
        return null;
      }
      else if (customization.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Customization>(customization, _appContext.UserId, _appContext.World);
      }

      return _mapper.Map<CustomizationModel>(customization);
    }
  }
}
