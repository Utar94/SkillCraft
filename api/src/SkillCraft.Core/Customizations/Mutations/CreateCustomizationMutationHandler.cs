using AutoMapper;
using MediatR;
using SkillCraft.Core.Customizations.Models;

namespace SkillCraft.Core.Customizations.Mutations
{
  internal class CreateCustomizationMutationHandler : SaveCustomizationHandler, IRequestHandler<CreateCustomizationMutation, CustomizationModel>
  {
    public CreateCustomizationMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<CustomizationModel> Handle(CreateCustomizationMutation request, CancellationToken cancellationToken)
    {
      var customization = new Customization(request.Payload.Type, AppContext.UserId, AppContext.World);

      DbContext.Customizations.Add(customization);

      return await ExecuteAsync(customization, request.Payload, cancellationToken);
    }
  }
}
