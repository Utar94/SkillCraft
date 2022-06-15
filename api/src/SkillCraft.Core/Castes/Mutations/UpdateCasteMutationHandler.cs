using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Castes.Models;

namespace SkillCraft.Core.Castes.Mutations
{
  internal class UpdateCasteMutationHandler : SaveCasteHandler, IRequestHandler<UpdateCasteMutation, CasteModel>
  {
    public UpdateCasteMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<CasteModel> Handle(UpdateCasteMutation request, CancellationToken cancellationToken)
    {
      Caste caste = await DbContext.Castes
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Caste>(request.Id);

      if (caste.WorldId != AppContext.World.Id)
      {
        throw new UnauthorizedOperationException<Caste>(caste, AppContext.UserId, AppContext.World);
      }

      caste.Update(AppContext.UserId);

      return await ExecuteAsync(caste, request.Payload, cancellationToken);
    }
  }
}
