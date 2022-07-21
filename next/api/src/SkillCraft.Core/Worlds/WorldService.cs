using AutoMapper;
using FluentValidation;
using SkillCraft.Core.Worlds.Models;
using SkillCraft.Core.Worlds.Payload;

namespace SkillCraft.Core.Worlds
{
  internal class WorldService : IWorldService
  {
    private readonly IMapper _mapper;
    private readonly IWorldQuerier _querier;
    private readonly IRepository<World> _repository;
    private readonly IUserContext _userContext;
    private readonly IValidator<World> _validator;

    public WorldService(
      IMapper mapper,
      IWorldQuerier querier,
      IRepository<World> repository,
      IUserContext userContext,
      IValidator<World> validator
    )
    {
      _mapper = mapper;
      _querier = querier;
      _repository = repository;
      _userContext = userContext;
      _validator = validator;
    }

    public async Task<WorldModel> CreateAsync(CreateWorldPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      if (await _querier.GetAsync(payload.Alias, readOnly: true, cancellationToken) != null)
      {
        throw new AliasAlreadyUsedException(payload.Alias, nameof(payload.Alias));
      }

      var world = new World(payload, _userContext.Id);
      _validator.ValidateAndThrow(world);

      await _repository.SaveAsync(world, cancellationToken);

      return _mapper.Map<WorldModel>(world);
    }

    public async Task<WorldModel> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      World world = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<World>(id);

      if (world.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<World>(world, _userContext.Id);
      }

      world.Delete(_userContext.Id);
      await _repository.SaveAsync(world, cancellationToken);

      return _mapper.Map<WorldModel>(world);
    }

    public async Task<WorldModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      World? world = await _querier.GetAsync(id, readOnly: true, cancellationToken);
      if (world == null)
      {
        return null;
      }
      else if (world.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<World>(world, _userContext.Id);
      }

      return _mapper.Map<WorldModel>(world);
    }

    public async Task<ListModel<WorldModel>> GetAsync(string? search,
      WorldSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      PagedList<World> worlds = await _querier.GetPagedAsync(_userContext.Id, search,
        sort, desc,
        index, count,
        readOnly: true, cancellationToken);

      return ListModel<WorldModel>.From(worlds, _mapper);
    }

    public async Task<WorldModel> UpdateAsync(Guid id, UpdateWorldPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      World world = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<World>(id);

      if (world.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<World>(world, _userContext.Id);
      }

      world.Update(payload, _userContext.Id);
      await _repository.SaveAsync(world, cancellationToken);

      return _mapper.Map<WorldModel>(world);
    }
  }
}
