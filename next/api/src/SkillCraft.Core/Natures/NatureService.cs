using AutoMapper;
using FluentValidation;
using SkillCraft.Core.Customizations;
using SkillCraft.Core.Natures.Models;
using SkillCraft.Core.Natures.Payload;

namespace SkillCraft.Core.Natures
{
  internal class NatureService : INatureService
  {
    private readonly ICustomizationQuerier _customizationQuerier;
    private readonly IMapper _mapper;
    private readonly INatureQuerier _querier;
    private readonly IRepository<Nature> _repository;
    private readonly IUserContext _userContext;
    private readonly IValidator<Nature> _validator;

    public NatureService(
      ICustomizationQuerier customizationQuerier,
      IMapper mapper,
      INatureQuerier querier,
      IRepository<Nature> repository,
      IUserContext userContext,
      IValidator<Nature> validator
    )
    {
      _customizationQuerier = customizationQuerier;
      _mapper = mapper;
      _querier = querier;
      _repository = repository;
      _userContext = userContext;
      _validator = validator;
    }

    public async Task<NatureModel> CreateAsync(CreateNaturePayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      Customization? feat = null;
      if (payload.FeatId.HasValue)
      {
        feat = await _customizationQuerier.GetAsync(payload.FeatId.Value, readOnly: false, cancellationToken)
          ?? throw new EntityNotFoundException<Customization>(payload.FeatId.Value, nameof(payload.FeatId));

        if (feat.WorldSid != _userContext.World.Sid)
        {
          throw new ForbiddenException<Customization>(feat, _userContext.Id);
        }
      }

      var nature = new Nature(payload, _userContext.Id, _userContext.World, feat);
      _validator.ValidateAndThrow(nature);

      await _repository.SaveAsync(nature, cancellationToken);

      return _mapper.Map<NatureModel>(nature);
    }

    public async Task<NatureModel> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Nature nature = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Nature>(id);

      if (nature.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Nature>(nature, _userContext.Id);
      }

      nature.Delete(_userContext.Id);
      await _repository.SaveAsync(nature, cancellationToken);

      return _mapper.Map<NatureModel>(nature);
    }

    public async Task<NatureModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Nature? nature = await _querier.GetAsync(id, readOnly: true, cancellationToken);
      if (nature == null)
      {
        return null;
      }
      else if (nature.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Nature>(nature, _userContext.Id);
      }

      return _mapper.Map<NatureModel>(nature);
    }

    public async Task<ListModel<NatureModel>> GetAsync(Attribute? attribute, string? search,
      NatureSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      PagedList<Nature> natures = await _querier.GetPagedAsync(_userContext.World.Sid, attribute, search,
        sort, desc,
        index, count,
        readOnly: true, cancellationToken);

      return ListModel<NatureModel>.From(natures, _mapper);
    }

    public async Task<NatureModel> UpdateAsync(Guid id, UpdateNaturePayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      Customization? feat = null;
      if (payload.FeatId.HasValue)
      {
        feat = await _customizationQuerier.GetAsync(payload.FeatId.Value, readOnly: false, cancellationToken)
          ?? throw new EntityNotFoundException<Customization>(payload.FeatId.Value, nameof(payload.FeatId));

        if (feat.WorldSid != _userContext.World.Sid)
        {
          throw new ForbiddenException<Customization>(feat, _userContext.Id);
        }
      }

      Nature nature = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Nature>(id);

      if (nature.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Nature>(nature, _userContext.Id);
      }

      nature.Update(payload, _userContext.Id, feat);
      await _repository.SaveAsync(nature, cancellationToken);

      return _mapper.Map<NatureModel>(nature);
    }
  }
}
