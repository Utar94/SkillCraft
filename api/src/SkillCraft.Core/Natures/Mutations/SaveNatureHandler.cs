using AutoMapper;
using Logitar;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Customizations;
using SkillCraft.Core.Natures.Models;
using SkillCraft.Core.Natures.Payloads;

namespace SkillCraft.Core.Natures.Mutations
{
  internal abstract class SaveNatureHandler
  {
    protected SaveNatureHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      AppContext = appContext;
      DbContext = dbContext;
      Mapper = mapper;
    }

    protected IApplicationContext AppContext { get; }
    protected IDbContext DbContext { get; }
    protected IMapper Mapper { get; }

    protected async Task<NatureModel> ExecuteAsync(Nature nature, SaveNaturePayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(nature);
      ArgumentNullException.ThrowIfNull(payload);

      Customization customization = await DbContext.Customizations
        .SingleOrDefaultAsync(x => x.Uuid == payload.FeatId, cancellationToken)
        ?? throw new EntityNotFoundException<Customization>(payload.FeatId, nameof(payload.FeatId));

      if (customization.WorldId != AppContext.World.Id)
      {
        throw new UnauthorizedOperationException<Customization>(customization, AppContext.UserId, AppContext.World);
      }
      else if (customization.Type != CustomizationType.Feat)
      {
        throw new InvalidCustomizationTypeException(customization, CustomizationType.Feat);
      }

      nature.Description = payload.Description?.CleanTrim();
      nature.Name = payload.Name.Trim();

      nature.Attribute = payload.Attribute;
      nature.Feat = customization;
      nature.FeatId = customization.Id;

      await DbContext.SaveChangesAsync(cancellationToken);

      AppContext.SetEntity(nature);

      return Mapper.Map<NatureModel>(nature);
    }
  }
}
