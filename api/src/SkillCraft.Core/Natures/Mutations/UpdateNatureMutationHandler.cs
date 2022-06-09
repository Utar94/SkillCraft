using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Natures.Models;

namespace SkillCraft.Core.Natures.Mutations
{
  internal class UpdateNatureMutationHandler : SaveNatureHandler, IRequestHandler<UpdateNatureMutation, NatureModel>
  {
    public UpdateNatureMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<NatureModel> Handle(UpdateNatureMutation request, CancellationToken cancellationToken)
    {
      Nature nature = await DbContext.Natures
        .Include(x => x.Feat)
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Nature>(request.Id);

      if (nature.WorldId != AppContext.World.Id)
      {
        throw new UnauthorizedOperationException<Nature>(nature, AppContext.UserId, AppContext.World);
      }

      nature.Update(AppContext.UserId);

      return await ExecuteAsync(nature, request.Payload, cancellationToken);
    }
  }
}
