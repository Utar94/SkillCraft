using AutoMapper;
using Logitar;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Classes.Models;
using SkillCraft.Core.Classes.Payloads;
using SkillCraft.Core.Talents;

namespace SkillCraft.Core.Classes.Mutations
{
  internal abstract class SaveClassHandler
  {
    protected SaveClassHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      AppContext = appContext;
      DbContext = dbContext;
      Mapper = mapper;
    }

    protected IApplicationContext AppContext { get; }
    protected IDbContext DbContext { get; }
    protected IMapper Mapper { get; }

    protected async Task<ClassModel> ExecuteAsync(Class @class, SaveClassPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(@class);
      ArgumentNullException.ThrowIfNull(payload);

      @class.Name = payload.Name.Trim();
      @class.Description = payload.Description?.CleanTrim();

      @class.OtherRequirements = payload.OtherRequirements?.CleanTrim();
      @class.OtherTalentsText = payload.OtherTalentsText?.CleanTrim();

      await UpdateTalentsAsync(@class, payload, cancellationToken);
      @class.Validate();

      await DbContext.SaveChangesAsync(cancellationToken);

      AppContext.SetEntity(@class);

      return Mapper.Map<ClassModel>(@class);
    }

    private async Task UpdateTalentsAsync(Class @class, SaveClassPayload payload, CancellationToken cancellationToken)
    {
      Dictionary<int, ClassTalent> classTalents = @class.Talents
        .ToDictionary(x => x.TalentId, x => x);

      @class.Talents.Clear();

      HashSet<Guid> talentIds = (payload.Talents?.Select(x => x.TalentId) ?? Enumerable.Empty<Guid>())
        .Concat(new[] { payload.UniqueTalentId })
        .ToHashSet();
      Dictionary<Guid, Talent> talents = await DbContext.Talents
        .Include(x => x.Class)
        .Include(x => x.RequiredTalent)
        .Include(x => x.RequiringTalents)
        .Where(x => talentIds.Contains(x.Uuid))
        .ToDictionaryAsync(x => x.Uuid, x => x, cancellationToken);

      if (!talents.TryGetValue(payload.UniqueTalentId, out Talent? uniqueTalent))
      {
        throw new EntityNotFoundException<Talent>(payload.UniqueTalentId, nameof(payload.UniqueTalentId));
      }
      else if (uniqueTalent.WorldId != AppContext.World.Id)
      {
        throw new UnauthorizedOperationException<Talent>(uniqueTalent, AppContext.UserId, AppContext.World);
      }
      else if (uniqueTalent.Class?.Id.Equals(@class.Id) == false)
      {
        throw new UniqueTalentAlreadyUsedException(uniqueTalent, nameof(payload.UniqueTalentId));
      }
      else if (uniqueTalent.RequiredTalent != null)
      {
        throw new UniqueTalentCannotRequireException(uniqueTalent);
      }
      else if (uniqueTalent.RequiringTalents.Any())
      {
        throw new UniqueTalentCannotBeRequiredException(uniqueTalent);
      }
      else if (uniqueTalent.Tier >= @class.Tier)
      {
        throw new InvalidClassTalentTierException(@class.Tier, uniqueTalent);
      }

      @class.UniqueTalent = uniqueTalent;
      @class.UniqueTalentId = uniqueTalent.Id;

      if (payload.Talents != null)
      {
        var missingIds = new List<Guid>(capacity: payload.Talents.Count());

        foreach (ClassTalentPayload talentPayload in payload.Talents)
        {
          if (!talents.TryGetValue(talentPayload.TalentId, out Talent? talent))
          {
            missingIds.Add(talentPayload.TalentId);

            continue;
          }
          else if (talent.WorldId != AppContext.World.Id)
          {
            throw new UnauthorizedOperationException<Talent>(talent, AppContext.UserId, AppContext.World);
          }
          else if (talent.Class != null)
          {
            throw new InvalidUniqueClassTalentException(talent);
          }
          else if (talent.Tier >= @class.Tier)
          {
            throw new InvalidClassTalentTierException(@class.Tier, talent);
          }

          if (!classTalents.TryGetValue(talent.Id, out ClassTalent? classTalent))
          {
            classTalent = new ClassTalent(@class, talent);
          }

          classTalent.Mandatory = talentPayload.Mandatory;

          @class.Talents.Add(classTalent);
        }

        if (missingIds.Any())
        {
          throw new TalentsNotFoundException(missingIds);
        }
      }
    }
  }
}
