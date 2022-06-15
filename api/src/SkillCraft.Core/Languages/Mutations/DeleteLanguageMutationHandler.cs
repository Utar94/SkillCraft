using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Languages.Models;

namespace SkillCraft.Core.Languages.Mutations
{
  internal class DeleteLanguageMutationHandler : IRequestHandler<DeleteLanguageMutation, LanguageModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public DeleteLanguageMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<LanguageModel> Handle(DeleteLanguageMutation request, CancellationToken cancellationToken)
    {
      Language language = await _dbContext.Languages
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Language>(request.Id);

      if (language.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Language>(language, _appContext.UserId, _appContext.World);
      }

      language.Delete(_appContext.UserId);
      await _dbContext.SaveChangesAsync(cancellationToken);

      return _mapper.Map<LanguageModel>(language);
    }
  }
}
