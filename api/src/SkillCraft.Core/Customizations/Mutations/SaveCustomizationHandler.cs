using AutoMapper;
using Logitar;
using SkillCraft.Core.Customizations.Models;
using SkillCraft.Core.Customizations.Payloads;

namespace SkillCraft.Core.Customizations.Mutations
{
  internal abstract class SaveCustomizationHandler
  {
    protected SaveCustomizationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      AppContext = appContext;
      DbContext = dbContext;
      Mapper = mapper;
    }

    protected IApplicationContext AppContext { get; }
    protected IDbContext DbContext { get; }
    protected IMapper Mapper { get; }

    protected async Task<CustomizationModel> ExecuteAsync(Customization customization, SaveCustomizationPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(customization);
      ArgumentNullException.ThrowIfNull(payload);

      customization.Description = payload.Description?.CleanTrim();
      customization.Name = payload.Name.Trim();

      await DbContext.SaveChangesAsync(cancellationToken);

      AppContext.SetEntity(customization);

      return Mapper.Map<CustomizationModel>(customization);
    }
  }
}
