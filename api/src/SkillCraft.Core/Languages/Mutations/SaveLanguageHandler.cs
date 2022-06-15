using AutoMapper;
using Logitar;
using SkillCraft.Core.Languages.Models;
using SkillCraft.Core.Languages.Payloads;

namespace SkillCraft.Core.Languages.Mutations
{
  internal abstract class SaveLanguageHandler
  {
    protected SaveLanguageHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      AppContext = appContext;
      DbContext = dbContext;
      Mapper = mapper;
    }

    protected IApplicationContext AppContext { get; }
    protected IDbContext DbContext { get; }
    protected IMapper Mapper { get; }

    protected async Task<LanguageModel> ExecuteAsync(Language language, SaveLanguagePayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(language);
      ArgumentNullException.ThrowIfNull(payload);

      language.Description = payload.Description?.CleanTrim();
      language.Name = payload.Name.Trim();

      language.Exotic = payload.Exotic;
      language.Script = payload.Script?.CleanTrim();
      language.TypicalSpeakers = payload.TypicalSpeakers?.CleanTrim();

      await DbContext.SaveChangesAsync(cancellationToken);

      AppContext.SetEntity(language);

      return Mapper.Map<LanguageModel>(language);
    }
  }
}
