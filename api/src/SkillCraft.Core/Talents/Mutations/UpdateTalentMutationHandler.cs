using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Talents.Models;

namespace SkillCraft.Core.Talents.Mutations
{
  internal class UpdateTalentMutationHandler : SaveTalentHandler, IRequestHandler<UpdateTalentMutation, TalentModel>
  {
    public UpdateTalentMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<TalentModel> Handle(UpdateTalentMutation request, CancellationToken cancellationToken)
    {
      Talent talent = await DbContext.Talents
        .Include(x => x.RequiredTalent)
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Talent>(request.Id);

      if (talent.WorldId != AppContext.World.Id)
      {
        throw new UnauthorizedOperationException<Talent>(talent, AppContext.UserId, AppContext.World);
      }

      talent.Update(AppContext.UserId);

      return await ExecuteAsync(talent, request.Payload, cancellationToken);
    }
  }
}
