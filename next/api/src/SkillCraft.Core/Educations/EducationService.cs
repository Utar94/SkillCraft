using AutoMapper;
using FluentValidation;
using SkillCraft.Core.Educations.Models;
using SkillCraft.Core.Educations.Payload;

namespace SkillCraft.Core.Educations
{
  internal class EducationService : IEducationService
  {
    private readonly IMapper _mapper;
    private readonly IEducationQuerier _querier;
    private readonly IRepository<Education> _repository;
    private readonly IUserContext _userContext;
    private readonly IValidator<Education> _validator;

    public EducationService(
      IMapper mapper,
      IEducationQuerier querier,
      IRepository<Education> repository,
      IUserContext userContext,
      IValidator<Education> validator
    )
    {
      _mapper = mapper;
      _querier = querier;
      _repository = repository;
      _userContext = userContext;
      _validator = validator;
    }

    public async Task<EducationModel> CreateAsync(CreateEducationPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      var education = new Education(payload, _userContext.Id, _userContext.World);
      _validator.ValidateAndThrow(education);

      await _repository.SaveAsync(education, cancellationToken);

      return _mapper.Map<EducationModel>(education);
    }

    public async Task<EducationModel> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Education education = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Education>(id);

      if (education.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Education>(education, _userContext.Id);
      }

      education.Delete(_userContext.Id);
      await _repository.SaveAsync(education, cancellationToken);

      return _mapper.Map<EducationModel>(education);
    }

    public async Task<EducationModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Education? education = await _querier.GetAsync(id, readOnly: true, cancellationToken);
      if (education == null)
      {
        return null;
      }
      else if (education.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Education>(education, _userContext.Id);
      }

      return _mapper.Map<EducationModel>(education);
    }

    public async Task<ListModel<EducationModel>> GetAsync(string? search, Skill? skill,
      EducationSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      PagedList<Education> educations = await _querier.GetPagedAsync(_userContext.World.Sid, search, skill,
        sort, desc,
        index, count,
        readOnly: true, cancellationToken);

      return ListModel<EducationModel>.From(educations, _mapper);
    }

    public async Task<EducationModel> UpdateAsync(Guid id, UpdateEducationPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      Education education = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Education>(id);

      if (education.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Education>(education, _userContext.Id);
      }

      education.Update(payload, _userContext.Id);
      await _repository.SaveAsync(education, cancellationToken);

      return _mapper.Map<EducationModel>(education);
    }
  }
}
