using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Classes.Models;

namespace SkillCraft.Core.Classes.Mutations
{
  internal class DeleteClassMutationHandler : IRequestHandler<DeleteClassMutation, ClassModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public DeleteClassMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<ClassModel> Handle(DeleteClassMutation request, CancellationToken cancellationToken)
    {
      Class @class = await _dbContext.Classes
        .Include(x => x.Talents).ThenInclude(x => x.Talent)
        .Include(x => x.UniqueTalent)
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Class>(request.Id);

      if (@class.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Class>(@class, _appContext.UserId, _appContext.World);
      }

      @class.Delete(_appContext.UserId);
      await _dbContext.SaveChangesAsync(cancellationToken);

      return _mapper.Map<ClassModel>(@class);
    }
  }
}
