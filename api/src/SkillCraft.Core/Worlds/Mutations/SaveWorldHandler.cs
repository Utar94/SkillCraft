using AutoMapper;
using Logitar;
using SkillCraft.Core.Worlds.Models;
using SkillCraft.Core.Worlds.Payloads;

namespace SkillCraft.Core.Worlds.Mutations
{
  internal abstract class SaveWorldHandler
  {
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    protected SaveWorldHandler(IDbContext dbContext, IMapper mapper)
    {
      _dbContext = dbContext;
      _mapper = mapper;
    }

    protected async Task<WorldModel> ExecuteAsync(World world, SaveWorldPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(world);
      ArgumentNullException.ThrowIfNull(payload);

      world.Description = payload.Description?.CleanTrim();
      world.Name = payload.Name.Trim();

      await _dbContext.SaveChangesAsync(cancellationToken);

      return _mapper.Map<WorldModel>(world);
    }
  }
}
