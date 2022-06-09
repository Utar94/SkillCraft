using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Worlds.Models;

namespace SkillCraft.Core.Worlds.Mutations
{
  internal class CreateWorldMutationHandler : SaveWorldHandler, IRequestHandler<CreateWorldMutation, WorldModel>
  {
    public CreateWorldMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<WorldModel> Handle(CreateWorldMutation request, CancellationToken cancellationToken)
    {
      string alias = request.Payload.Alias.ToLowerInvariant();
      if (await DbContext.Worlds.AnyAsync(x => x.Alias == alias, cancellationToken))
      {
        throw new AliasAlreadyUsedException(alias, nameof(request.Payload.Alias));
      }

      var world = new World(alias, AppContext.UserId);
      DbContext.Worlds.Add(world);

      return await ExecuteAsync(world, request.Payload, cancellationToken);
    }
  }
}
