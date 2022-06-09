using AutoMapper;
using Logitar;
using SkillCraft.Core.Aspects.Models;
using SkillCraft.Core.Aspects.Payloads;

namespace SkillCraft.Core.Aspects.Mutations
{
  internal abstract class SaveAspectHandler
  {
    protected SaveAspectHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      AppContext = appContext;
      DbContext = dbContext;
      Mapper = mapper;
    }

    protected IApplicationContext AppContext { get; }
    protected IDbContext DbContext { get; }
    protected IMapper Mapper { get; }

    protected async Task<AspectModel> ExecuteAsync(Aspect aspect, SaveAspectPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(aspect);
      ArgumentNullException.ThrowIfNull(payload);

      aspect.Description = payload.Description?.CleanTrim();
      aspect.Name = payload.Name.Trim();

      aspect.MandatoryAttribute1 = payload.MandatoryAttribute1;
      aspect.MandatoryAttribute2 = payload.MandatoryAttribute2;
      aspect.OptionalAttribute1 = payload.OptionalAttribute1;
      aspect.OptionalAttribute2 = payload.OptionalAttribute2;
      aspect.Skill1 = payload.Skill1;
      aspect.Skill2 = payload.Skill2;

      await DbContext.SaveChangesAsync(cancellationToken);

      AppContext.SetEntity(aspect);

      return Mapper.Map<AspectModel>(aspect);
    }
  }
}
