using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Natures.Models;

namespace SkillCraft.Core.Natures.Mutations
{
  internal class DeleteNatureMutationHandler : IRequestHandler<DeleteNatureMutation, NatureModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public DeleteNatureMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<NatureModel> Handle(DeleteNatureMutation request, CancellationToken cancellationToken)
    {
      Nature nature = await _dbContext.Natures
        .Include(x => x.Feat)
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Nature>(request.Id);

      if (nature.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Nature>(nature, _appContext.UserId, _appContext.World);
      }

      nature.Delete(_appContext.UserId);
      await _dbContext.SaveChangesAsync(cancellationToken);

      return _mapper.Map<NatureModel>(nature);
    }
  }
}
