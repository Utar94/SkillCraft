using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Castes.Models;

namespace SkillCraft.Core.Castes.Mutations
{
  internal class DeleteCasteMutationHandler : IRequestHandler<DeleteCasteMutation, CasteModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public DeleteCasteMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<CasteModel> Handle(DeleteCasteMutation request, CancellationToken cancellationToken)
    {
      Caste caste = await _dbContext.Castes
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Caste>(request.Id);

      if (caste.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Caste>(caste, _appContext.UserId, _appContext.World);
      }

      caste.Delete(_appContext.UserId);
      await _dbContext.SaveChangesAsync(cancellationToken);

      return _mapper.Map<CasteModel>(caste);
    }
  }
}
