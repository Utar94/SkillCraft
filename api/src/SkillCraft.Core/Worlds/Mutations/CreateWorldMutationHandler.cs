using AutoMapper;
using Logitar.Identity.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Worlds.Models;

namespace SkillCraft.Core.Worlds.Mutations
{
  internal class CreateWorldMutationHandler : SaveWorldHandler, IRequestHandler<CreateWorldMutation, WorldModel>
  {
    private readonly IDbContext _dbContext;
    private readonly IUserContext _userContext;

    public CreateWorldMutationHandler(IDbContext dbContext, IMapper mapper, IUserContext userContext)
      : base(dbContext, mapper)
    {
      _dbContext = dbContext;
      _userContext = userContext;
    }

    public async Task<WorldModel> Handle(CreateWorldMutation request, CancellationToken cancellationToken)
    {
      string alias = request.Payload.Alias.ToLowerInvariant();
      if (await _dbContext.Worlds.AnyAsync(x => x.Alias == alias, cancellationToken))
      {
        throw new AliasAlreadyUsedException(alias, nameof(request.Payload.Alias));
      }

      var world = new World(alias, _userContext.Id);
      _dbContext.Worlds.Add(world);

      return await ExecuteAsync(world, request.Payload, cancellationToken);
    }
  }
}
