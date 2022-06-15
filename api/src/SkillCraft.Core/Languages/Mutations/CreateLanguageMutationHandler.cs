using AutoMapper;
using MediatR;
using SkillCraft.Core.Languages.Models;

namespace SkillCraft.Core.Languages.Mutations
{
  internal class CreateLanguageMutationHandler : SaveLanguageHandler, IRequestHandler<CreateLanguageMutation, LanguageModel>
  {
    public CreateLanguageMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<LanguageModel> Handle(CreateLanguageMutation request, CancellationToken cancellationToken)
    {
      var language = new Language(AppContext.UserId, AppContext.World);

      DbContext.Languages.Add(language);

      return await ExecuteAsync(language, request.Payload, cancellationToken);
    }
  }
}
