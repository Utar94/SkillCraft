using AutoMapper;
using MediatR;
using SkillCraft.Core.Castes.Models;

namespace SkillCraft.Core.Castes.Mutations
{
  internal class CreateCasteMutationHandler : SaveCasteHandler, IRequestHandler<CreateCasteMutation, CasteModel>
  {
    public CreateCasteMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<CasteModel> Handle(CreateCasteMutation request, CancellationToken cancellationToken)
    {
      var caste = new Caste(AppContext.UserId, AppContext.World);

      DbContext.Castes.Add(caste);

      return await ExecuteAsync(caste, request.Payload, cancellationToken);
    }
  }
}
