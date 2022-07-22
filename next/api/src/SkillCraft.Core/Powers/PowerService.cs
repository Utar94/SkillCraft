using AutoMapper;
using FluentValidation;
using SkillCraft.Core.Powers.Models;
using SkillCraft.Core.Powers.Payload;

namespace SkillCraft.Core.Powers
{
  internal class PowerService : IPowerService
  {
    private readonly IMapper _mapper;
    private readonly IPowerQuerier _querier;
    private readonly IRepository<Power> _repository;
    private readonly IUserContext _userContext;
    private readonly IValidator<Power> _validator;

    public PowerService(
      IMapper mapper,
      IPowerQuerier querier,
      IRepository<Power> repository,
      IUserContext userContext,
      IValidator<Power> validator
    )
    {
      _mapper = mapper;
      _querier = querier;
      _repository = repository;
      _userContext = userContext;
      _validator = validator;
    }

    public async Task<PowerModel> CreateAsync(CreatePowerPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      var power = new Power(payload, _userContext.Id, _userContext.World);
      _validator.ValidateAndThrow(power);

      await _repository.SaveAsync(power, cancellationToken);

      return _mapper.Map<PowerModel>(power);
    }

    public async Task<PowerModel> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Power power = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Power>(id);

      if (power.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Power>(power, _userContext.Id);
      }

      power.Delete(_userContext.Id);
      await _repository.SaveAsync(power, cancellationToken);

      return _mapper.Map<PowerModel>(power);
    }

    public async Task<PowerModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Power? power = await _querier.GetAsync(id, readOnly: true, cancellationToken);
      if (power == null)
      {
        return null;
      }
      else if (power.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Power>(power, _userContext.Id);
      }

      return _mapper.Map<PowerModel>(power);
    }

    public async Task<ListModel<PowerModel>> GetAsync(string? search, IEnumerable<int>? tiers,
      PowerSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      PagedList<Power> powers = await _querier.GetPagedAsync(_userContext.World.Sid, search, tiers,
        sort, desc,
        index, count,
        readOnly: true, cancellationToken);

      return ListModel<PowerModel>.From(powers, _mapper);
    }

    public async Task<PowerModel> UpdateAsync(Guid id, UpdatePowerPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      Power power = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Power>(id);

      if (power.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Power>(power, _userContext.Id);
      }

      power.Update(payload, _userContext.Id);
      await _repository.SaveAsync(power, cancellationToken);

      return _mapper.Map<PowerModel>(power);
    }
  }
}
