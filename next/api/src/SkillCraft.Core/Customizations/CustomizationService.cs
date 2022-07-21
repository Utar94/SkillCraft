using AutoMapper;
using FluentValidation;
using SkillCraft.Core.Customizations.Models;
using SkillCraft.Core.Customizations.Payload;

namespace SkillCraft.Core.Customizations
{
  internal class CustomizationService : ICustomizationService
  {
    private readonly IMapper _mapper;
    private readonly ICustomizationQuerier _querier;
    private readonly IRepository<Customization> _repository;
    private readonly IUserContext _userContext;
    private readonly IValidator<Customization> _validator;

    public CustomizationService(
      IMapper mapper,
      ICustomizationQuerier querier,
      IRepository<Customization> repository,
      IUserContext userContext,
      IValidator<Customization> validator
    )
    {
      _mapper = mapper;
      _querier = querier;
      _repository = repository;
      _userContext = userContext;
      _validator = validator;
    }

    public async Task<CustomizationModel> CreateAsync(CreateCustomizationPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      var customization = new Customization(payload, _userContext.Id, _userContext.World);
      _validator.ValidateAndThrow(customization);

      await _repository.SaveAsync(customization, cancellationToken);

      return _mapper.Map<CustomizationModel>(customization);
    }

    public async Task<CustomizationModel> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Customization customization = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Customization>(id);

      if (customization.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Customization>(customization, _userContext.Id);
      }

      customization.Delete(_userContext.Id);
      await _repository.SaveAsync(customization, cancellationToken);

      return _mapper.Map<CustomizationModel>(customization);
    }

    public async Task<CustomizationModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Customization? customization = await _querier.GetAsync(id, readOnly: true, cancellationToken);
      if (customization == null)
      {
        return null;
      }
      else if (customization.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Customization>(customization, _userContext.Id);
      }

      return _mapper.Map<CustomizationModel>(customization);
    }

    public async Task<ListModel<CustomizationModel>> GetAsync(string? search, CustomizationType? type,
      CustomizationSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      PagedList<Customization> customizations = await _querier.GetPagedAsync(_userContext.World.Sid, search, type,
        sort, desc,
        index, count,
        readOnly: true, cancellationToken);

      return ListModel<CustomizationModel>.From(customizations, _mapper);
    }

    public async Task<CustomizationModel> UpdateAsync(Guid id, UpdateCustomizationPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      Customization customization = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Customization>(id);

      if (customization.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Customization>(customization, _userContext.Id);
      }

      customization.Update(payload, _userContext.Id);
      await _repository.SaveAsync(customization, cancellationToken);

      return _mapper.Map<CustomizationModel>(customization);
    }
  }
}
