using AutoMapper;
using FluentValidation;
using SkillCraft.Core.Aspects.Models;
using SkillCraft.Core.Aspects.Payload;

namespace SkillCraft.Core.Aspects
{
  internal class AspectService : IAspectService
  {
    private readonly IMapper _mapper;
    private readonly IAspectQuerier _querier;
    private readonly IRepository<Aspect> _repository;
    private readonly IUserContext _userContext;
    private readonly IValidator<Aspect> _validator;

    public AspectService(
      IMapper mapper,
      IAspectQuerier querier,
      IRepository<Aspect> repository,
      IUserContext userContext,
      IValidator<Aspect> validator
    )
    {
      _mapper = mapper;
      _querier = querier;
      _repository = repository;
      _userContext = userContext;
      _validator = validator;
    }

    public async Task<AspectModel> CreateAsync(CreateAspectPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      var aspect = new Aspect(payload, _userContext.Id, _userContext.World);
      _validator.ValidateAndThrow(aspect);

      await _repository.SaveAsync(aspect, cancellationToken);

      return _mapper.Map<AspectModel>(aspect);
    }

    public async Task<AspectModel> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Aspect aspect = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Aspect>(id);

      if (aspect.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Aspect>(aspect, _userContext.Id);
      }

      aspect.Delete(_userContext.Id);
      await _repository.SaveAsync(aspect, cancellationToken);

      return _mapper.Map<AspectModel>(aspect);
    }

    public async Task<AspectModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Aspect? aspect = await _querier.GetAsync(id, readOnly: true, cancellationToken);
      if (aspect == null)
      {
        return null;
      }
      else if (aspect.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Aspect>(aspect, _userContext.Id);
      }

      return _mapper.Map<AspectModel>(aspect);
    }

    public async Task<ListModel<AspectModel>> GetAsync(string? search,
      AspectSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      PagedList<Aspect> aspects = await _querier.GetPagedAsync(_userContext.World.Sid, search,
        sort, desc,
        index, count,
        readOnly: true, cancellationToken);

      return ListModel<AspectModel>.From(aspects, _mapper);
    }

    public async Task<AspectModel> UpdateAsync(Guid id, UpdateAspectPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      Aspect aspect = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Aspect>(id);

      if (aspect.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Aspect>(aspect, _userContext.Id);
      }

      aspect.Update(payload, _userContext.Id);
      await _repository.SaveAsync(aspect, cancellationToken);

      return _mapper.Map<AspectModel>(aspect);
    }
  }
}
