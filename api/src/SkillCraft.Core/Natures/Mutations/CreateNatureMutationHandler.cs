using AutoMapper;
using MediatR;
using SkillCraft.Core.Natures.Models;

namespace SkillCraft.Core.Natures.Mutations
{
  internal class CreateNatureMutationHandler : SaveNatureHandler, IRequestHandler<CreateNatureMutation, NatureModel>
  {
    public CreateNatureMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<NatureModel> Handle(CreateNatureMutation request, CancellationToken cancellationToken)
    {
      var nature = new Nature(AppContext.UserId, AppContext.World);

      DbContext.Natures.Add(nature);

      return await ExecuteAsync(nature, request.Payload, cancellationToken);
    }
  }
}
