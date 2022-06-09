using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Aspects.Models;

namespace SkillCraft.Core.Aspects.Mutations
{
  internal class DeleteAspectMutationHandler : IRequestHandler<DeleteAspectMutation, AspectModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public DeleteAspectMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<AspectModel> Handle(DeleteAspectMutation request, CancellationToken cancellationToken)
    {
      Aspect aspect = await _dbContext.Aspects
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Aspect>(request.Id);

      if (aspect.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Aspect>(aspect, _appContext.UserId, _appContext.World);
      }

      aspect.Delete(_appContext.UserId);
      await _dbContext.SaveChangesAsync(cancellationToken);

      return _mapper.Map<AspectModel>(aspect);
    }
  }
}
