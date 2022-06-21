using AutoMapper;
using Logitar;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Talents.Models;
using SkillCraft.Core.Talents.Payloads;

namespace SkillCraft.Core.Talents.Mutations
{
  internal abstract class SaveTalentHandler
  {
    protected SaveTalentHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      AppContext = appContext;
      DbContext = dbContext;
      Mapper = mapper;
    }

    protected IApplicationContext AppContext { get; }
    protected IDbContext DbContext { get; }
    protected IMapper Mapper { get; }

    protected async Task<TalentModel> ExecuteAsync(Talent talent, SaveTalentPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(talent);
      ArgumentNullException.ThrowIfNull(payload);

      Talent? requiredTalent = null;
      if (payload.RequiredTalentId.HasValue)
      {
        requiredTalent = await DbContext.Talents
          .SingleOrDefaultAsync(x => x.Uuid == payload.RequiredTalentId.Value, cancellationToken)
          ?? throw new EntityNotFoundException<Talent>(payload.RequiredTalentId.Value, nameof(payload.RequiredTalentId));

        if (requiredTalent.WorldId != AppContext.World.Id)
        {
          throw new UnauthorizedOperationException<Talent>(talent, AppContext.UserId, AppContext.World);
        }
        else if (requiredTalent.Tier > talent.Tier)
        {
          throw new InvalidRequiredTalentTierException(requiredTalent, talent);
        }
      }

      talent.MultipleAcquisition = payload.MultipleAcquisition;
      talent.RequiredTalent = requiredTalent;
      talent.RequiredTalentId = requiredTalent?.Id;

      talent.Description = payload.Description?.CleanTrim();
      talent.Name = payload.Name.Trim();

      await DbContext.SaveChangesAsync(cancellationToken);

      AppContext.SetEntity(talent);

      return Mapper.Map<TalentModel>(talent);
    }
  }
}
