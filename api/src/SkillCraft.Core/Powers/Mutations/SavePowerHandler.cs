using AutoMapper;
using Logitar;
using SkillCraft.Core.Powers.Models;
using SkillCraft.Core.Powers.Payloads;

namespace SkillCraft.Core.Powers.Mutations
{
  internal abstract class SavePowerHandler
  {
    protected SavePowerHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      AppContext = appContext;
      DbContext = dbContext;
      Mapper = mapper;
    }

    protected IApplicationContext AppContext { get; }
    protected IDbContext DbContext { get; }
    protected IMapper Mapper { get; }

    protected async Task<PowerModel> ExecuteAsync(Power power, SavePowerPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(power);
      ArgumentNullException.ThrowIfNull(payload);

      power.Name = payload.Name.Trim();

      power.Incantation = payload.Incantation;
      power.Duration = payload.Duration;
      power.Range = payload.Range;
      power.Ingredients = payload.Ingredients?.CleanTrim();

      power.Concentration = payload.Concentration;
      power.Focus = payload.Focus;
      power.Ritual = payload.Ritual;
      power.Somatic = payload.Somatic;
      power.Verbal = payload.Verbal;

      UpdateDescriptions(power, payload);

      await DbContext.SaveChangesAsync(cancellationToken);

      AppContext.SetEntity(power);

      return Mapper.Map<PowerModel>(power);
    }

    private static void UpdateDescriptions(Power power, SavePowerPayload payload)
    {
      power.Descriptions = payload.Descriptions == null ? null : new[]
      {
        payload.Descriptions.Global,
        payload.Descriptions.FirstLevel,
        payload.Descriptions.SecondLevel,
        payload.Descriptions.ThirdLevel
      }.Where(desc => !string.IsNullOrWhiteSpace(desc)).Select(desc => desc!.Trim()).ToArray();
    }
  }
}
