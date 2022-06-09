using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Customizations.Models;

namespace SkillCraft.Core.Customizations.Mutations
{
  internal class UpdateCustomizationMutationHandler : SaveCustomizationHandler, IRequestHandler<UpdateCustomizationMutation, CustomizationModel>
  {
    public UpdateCustomizationMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<CustomizationModel> Handle(UpdateCustomizationMutation request, CancellationToken cancellationToken)
    {
      Customization customization = await DbContext.Customizations
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Customization>(request.Id);

      if (customization.WorldId != AppContext.World.Id)
      {
        throw new UnauthorizedOperationException<Customization>(customization, AppContext.UserId, AppContext.World);
      }

      customization.Update(AppContext.UserId);

      return await ExecuteAsync(customization, request.Payload, cancellationToken);
    }
  }
}
