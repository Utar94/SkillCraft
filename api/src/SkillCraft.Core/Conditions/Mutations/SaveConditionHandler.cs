using AutoMapper;
using Logitar;
using SkillCraft.Core.Conditions.Models;
using SkillCraft.Core.Conditions.Payloads;

namespace SkillCraft.Core.Conditions.Mutations
{
  internal abstract class SaveConditionHandler
  {
    protected SaveConditionHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      AppContext = appContext;
      DbContext = dbContext;
      Mapper = mapper;
    }

    protected IApplicationContext AppContext { get; }
    protected IDbContext DbContext { get; }
    protected IMapper Mapper { get; }

    protected async Task<ConditionModel> ExecuteAsync(Condition condition, SaveConditionPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(condition);
      ArgumentNullException.ThrowIfNull(payload);

      condition.Description = payload.Description?.CleanTrim();
      condition.Name = payload.Name.Trim();

      condition.MaxLevel = payload.MaxLevel;

      await DbContext.SaveChangesAsync(cancellationToken);

      AppContext.SetEntity(condition);

      return Mapper.Map<ConditionModel>(condition);
    }
  }
}
