using AutoMapper;
using FluentValidation;
using SkillCraft.Core.Languages.Models;
using SkillCraft.Core.Languages.Payload;

namespace SkillCraft.Core.Languages
{
  internal class LanguageService : ILanguageService
  {
    private readonly IMapper _mapper;
    private readonly ILanguageQuerier _querier;
    private readonly IRepository<Language> _repository;
    private readonly IUserContext _userContext;
    private readonly IValidator<Language> _validator;

    public LanguageService(
      IMapper mapper,
      ILanguageQuerier querier,
      IRepository<Language> repository,
      IUserContext userContext,
      IValidator<Language> validator
    )
    {
      _mapper = mapper;
      _querier = querier;
      _repository = repository;
      _userContext = userContext;
      _validator = validator;
    }

    public async Task<LanguageModel> CreateAsync(CreateLanguagePayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      var language = new Language(payload, _userContext.Id, _userContext.World);
      _validator.ValidateAndThrow(language);

      await _repository.SaveAsync(language, cancellationToken);

      return _mapper.Map<LanguageModel>(language);
    }

    public async Task<LanguageModel> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Language language = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Language>(id);

      if (language.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Language>(language, _userContext.Id);
      }

      language.Delete(_userContext.Id);
      await _repository.SaveAsync(language, cancellationToken);

      return _mapper.Map<LanguageModel>(language);
    }

    public async Task<LanguageModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Language? language = await _querier.GetAsync(id, readOnly: true, cancellationToken);
      if (language == null)
      {
        return null;
      }
      else if (language.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Language>(language, _userContext.Id);
      }

      return _mapper.Map<LanguageModel>(language);
    }

    public async Task<ListModel<LanguageModel>> GetAsync(bool? isExotic, string? search,
      LanguageSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      PagedList<Language> languages = await _querier.GetPagedAsync(_userContext.World.Sid, isExotic, search,
        sort, desc,
        index, count,
        readOnly: true, cancellationToken);

      return ListModel<LanguageModel>.From(languages, _mapper);
    }

    public async Task<LanguageModel> UpdateAsync(Guid id, UpdateLanguagePayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      Language language = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Language>(id);

      if (language.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Language>(language, _userContext.Id);
      }

      language.Update(payload, _userContext.Id);
      await _repository.SaveAsync(language, cancellationToken);

      return _mapper.Map<LanguageModel>(language);
    }
  }
}
