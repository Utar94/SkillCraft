using AutoMapper;
using FluentValidation;
using SkillCraft.Core.Castes.Models;
using SkillCraft.Core.Castes.Payload;

namespace SkillCraft.Core.Castes
{
  internal class CasteService : ICasteService
  {
    private readonly IMapper _mapper;
    private readonly ICasteQuerier _querier;
    private readonly IRepository<Caste> _repository;
    private readonly IUserContext _userContext;
    private readonly IValidator<Caste> _validator;

    public CasteService(
      IMapper mapper,
      ICasteQuerier querier,
      IRepository<Caste> repository,
      IUserContext userContext,
      IValidator<Caste> validator
    )
    {
      _mapper = mapper;
      _querier = querier;
      _repository = repository;
      _userContext = userContext;
      _validator = validator;
    }

    public async Task<CasteModel> CreateAsync(CreateCastePayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      var caste = new Caste(payload, _userContext.Id, _userContext.World);
      _validator.ValidateAndThrow(caste);

      await _repository.SaveAsync(caste, cancellationToken);

      return _mapper.Map<CasteModel>(caste);
    }

    public async Task<CasteModel> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Caste caste = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Caste>(id);

      if (caste.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Caste>(caste, _userContext.Id);
      }

      caste.Delete(_userContext.Id);
      await _repository.SaveAsync(caste, cancellationToken);

      return _mapper.Map<CasteModel>(caste);
    }

    public async Task<CasteModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Caste? caste = await _querier.GetAsync(id, readOnly: true, cancellationToken);
      if (caste == null)
      {
        return null;
      }
      else if (caste.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Caste>(caste, _userContext.Id);
      }

      return _mapper.Map<CasteModel>(caste);
    }

    public async Task<ListModel<CasteModel>> GetAsync(string? search, Skill? skill,
      CasteSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      PagedList<Caste> castes = await _querier.GetPagedAsync(_userContext.World.Sid, search, skill,
        sort, desc,
        index, count,
        readOnly: true, cancellationToken);

      return ListModel<CasteModel>.From(castes, _mapper);
    }

    public async Task<CasteModel> UpdateAsync(Guid id, UpdateCastePayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      Caste caste = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Caste>(id);

      if (caste.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Caste>(caste, _userContext.Id);
      }

      caste.Update(payload, _userContext.Id);
      await _repository.SaveAsync(caste, cancellationToken);

      return _mapper.Map<CasteModel>(caste);
    }
  }
}
