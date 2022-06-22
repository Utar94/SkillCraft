using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Talents.Models;

namespace SkillCraft.Core.Talents.Mutations
{
  internal class DeleteTalentMutationHandler : IRequestHandler<DeleteTalentMutation, TalentModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public DeleteTalentMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<TalentModel> Handle(DeleteTalentMutation request, CancellationToken cancellationToken)
    {
      Talent talent = await _dbContext.Talents
        .Include(x => x.Options)
        .Include(x => x.RequiredTalent)
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Talent>(request.Id);

      if (talent.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Talent>(talent, _appContext.UserId, _appContext.World);
      }

      talent.Delete(_appContext.UserId);
      await _dbContext.SaveChangesAsync(cancellationToken);

      return _mapper.Map<TalentModel>(talent);
    }
  }
}
