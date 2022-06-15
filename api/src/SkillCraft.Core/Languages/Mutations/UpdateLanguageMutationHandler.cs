using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Languages.Models;

namespace SkillCraft.Core.Languages.Mutations
{
  internal class UpdateLanguageMutationHandler : SaveLanguageHandler, IRequestHandler<UpdateLanguageMutation, LanguageModel>
  {
    public UpdateLanguageMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<LanguageModel> Handle(UpdateLanguageMutation request, CancellationToken cancellationToken)
    {
      Language language = await DbContext.Languages
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Language>(request.Id);

      if (language.WorldId != AppContext.World.Id)
      {
        throw new UnauthorizedOperationException<Language>(language, AppContext.UserId, AppContext.World);
      }

      language.Update(AppContext.UserId);

      return await ExecuteAsync(language, request.Payload, cancellationToken);
    }
  }
}
