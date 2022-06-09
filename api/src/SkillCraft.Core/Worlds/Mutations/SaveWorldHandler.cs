using AutoMapper;
using Logitar;
using SkillCraft.Core.Worlds.Models;
using SkillCraft.Core.Worlds.Payloads;

namespace SkillCraft.Core.Worlds.Mutations
{
  internal abstract class SaveWorldHandler
  {
    protected SaveWorldHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      AppContext = appContext;
      DbContext = dbContext;
      Mapper = mapper;
    }

    protected IApplicationContext AppContext { get; }
    protected IDbContext DbContext { get; }
    protected IMapper Mapper { get; }

    protected async Task<WorldModel> ExecuteAsync(World world, SaveWorldPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(world);
      ArgumentNullException.ThrowIfNull(payload);

      world.Description = payload.Description?.CleanTrim();
      world.Name = payload.Name.Trim();

      await DbContext.SaveChangesAsync(cancellationToken);

      AppContext.SetEntity(world);

      return Mapper.Map<WorldModel>(world);
    }
  }
}
