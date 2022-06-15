using AutoMapper;
using Logitar;
using SkillCraft.Core.Castes.Models;
using SkillCraft.Core.Castes.Payloads;

namespace SkillCraft.Core.Castes.Mutations
{
  internal abstract class SaveCasteHandler
  {
    protected SaveCasteHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      AppContext = appContext;
      DbContext = dbContext;
      Mapper = mapper;
    }

    protected IApplicationContext AppContext { get; }
    protected IDbContext DbContext { get; }
    protected IMapper Mapper { get; }

    protected async Task<CasteModel> ExecuteAsync(Caste caste, SaveCastePayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(caste);
      ArgumentNullException.ThrowIfNull(payload);

      caste.Description = payload.Description?.CleanTrim();
      caste.Name = payload.Name.Trim();

      caste.Skill = payload.Skill;
      caste.WealthRoll = payload.WealthRoll;

      await DbContext.SaveChangesAsync(cancellationToken);

      AppContext.SetEntity(caste);

      return Mapper.Map<CasteModel>(caste);
    }
  }
}
