using AutoMapper;
using MediatR;
using SkillCraft.Core.Talents.Models;

namespace SkillCraft.Core.Talents.Mutations
{
  internal class CreateTalentMutationHandler : SaveTalentHandler, IRequestHandler<CreateTalentMutation, TalentModel>
  {
    public CreateTalentMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<TalentModel> Handle(CreateTalentMutation request, CancellationToken cancellationToken)
    {
      var talent = new Talent(request.Payload.Tier, AppContext.UserId, AppContext.World);

      DbContext.Talents.Add(talent);

      return await ExecuteAsync(talent, request.Payload, cancellationToken);
    }
  }
}
