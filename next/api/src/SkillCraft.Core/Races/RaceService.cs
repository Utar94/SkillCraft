using AutoMapper;
using FluentValidation;
using SkillCraft.Core.Languages;
using SkillCraft.Core.Races.Models;
using SkillCraft.Core.Races.Payload;

namespace SkillCraft.Core.Races
{
  internal class RaceService : IRaceService
  {
    private readonly ILanguageQuerier _languageQuerier;
    private readonly IMapper _mapper;
    private readonly IRaceQuerier _querier;
    private readonly IRepository<Race> _repository;
    private readonly IUserContext _userContext;
    private readonly IValidator<Race> _validator;

    public RaceService(
      ILanguageQuerier languageQuerier,
      IMapper mapper,
      IRaceQuerier querier,
      IRepository<Race> repository,
      IUserContext userContext,
      IValidator<Race> validator
    )
    {
      _languageQuerier = languageQuerier;
      _mapper = mapper;
      _querier = querier;
      _repository = repository;
      _userContext = userContext;
      _validator = validator;
    }

    public async Task<RaceModel> CreateAsync(CreateRacePayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      Race? parent = null;
      if (payload.ParentId.HasValue)
      {
        parent = await _querier.GetAsync(payload.ParentId.Value, readOnly: false, cancellationToken)
          ?? throw new EntityNotFoundException<Race>(payload.ParentId.Value, nameof(payload.ParentId));

        if (parent.WorldSid != _userContext.World.Sid)
        {
          throw new ForbiddenException<Race>(parent, _userContext.Id);
        }
      }

      var race = new Race(payload, _userContext.Id, _userContext.World, parent);
      await UpdateLanguagesAndTraitsAsync(payload, race, cancellationToken);
      _validator.ValidateAndThrow(race);

      await _repository.SaveAsync(race, cancellationToken);

      return _mapper.Map<RaceModel>(race);
    }

    public async Task<RaceModel> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Race race = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Race>(id);

      if (race.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Race>(race, _userContext.Id);
      }

      race.Delete(_userContext.Id);
      await _repository.SaveAsync(race, cancellationToken);

      return _mapper.Map<RaceModel>(race);
    }

    public async Task<RaceModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Race? race = await _querier.GetAsync(id, readOnly: true, cancellationToken);
      if (race == null)
      {
        return null;
      }
      else if (race.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Race>(race, _userContext.Id);
      }

      return _mapper.Map<RaceModel>(race);
    }

    public async Task<ListModel<RaceModel>> GetAsync(Guid? parentId, string? search, SizeCategory? size,
      RaceSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      PagedList<Race> races = await _querier.GetPagedAsync(_userContext.World.Sid, parentId, search, size,
        sort, desc,
        index, count,
        readOnly: true, cancellationToken);

      return ListModel<RaceModel>.From(races, _mapper);
    }

    public async Task<RaceModel> UpdateAsync(Guid id, UpdateRacePayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      Race race = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Race>(id);

      if (race.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Race>(race, _userContext.Id);
      }

      race.Update(payload, _userContext.Id);
      await UpdateLanguagesAndTraitsAsync(payload, race, cancellationToken);
      _validator.ValidateAndThrow(race);

      await _repository.SaveAsync(race, cancellationToken);

      return _mapper.Map<RaceModel>(race);
    }

    private async Task<IEnumerable<Language>?> GetLanguagesAsync(SaveRacePayload payload, CancellationToken cancellationToken)
    {
      if (payload.LanguageIds == null)
      {
        return null;
      }

      IEnumerable<Language> languages = await _languageQuerier
        .GetAsync(_userContext.World.Sid, payload.LanguageIds, readOnly: false, cancellationToken);

      IEnumerable<Guid> missingLanguages = payload.LanguageIds.Except(languages.Select(x => x.Id));
      if (missingLanguages.Any())
      {
        throw new LanguagesNotFoundException(missingLanguages, nameof(payload.LanguageIds));
      }

      return languages;
    }

    private static IEnumerable<RacialTrait>? GetTraits(SaveRacePayload payload, Race race)
    {
      if (payload.Traits == null)
      {
        return null;
      }

      Dictionary<Guid, RacialTrait> racialTraits = race.Traits.ToDictionary(x => x.Id, x => x) ?? new();

      IEnumerable<Guid> missingTraits = payload.Traits.Where(x => x.Id.HasValue).Select(x => x.Id!.Value)
        .Except(racialTraits.Keys);
      if (missingTraits.Any())
      {
        throw new RacialTraitsNotFoundException(missingTraits, nameof(payload.Traits));
      }

      var traits = new List<RacialTrait>(capacity: payload.Traits.Count());

      foreach (RacialTraitPayload traitPayload in payload.Traits)
      {
        if (traitPayload.Id.HasValue)
        {
          RacialTrait trait = racialTraits[traitPayload.Id.Value];
          trait.Update(traitPayload.Name, traitPayload.Description);
          traits.Add(trait);
        }
        else
        {
          traits.Add(new RacialTrait(race, traitPayload.Name, traitPayload.Description));
        }
      }

      return traits;
    }

    private async Task UpdateLanguagesAndTraitsAsync(SaveRacePayload payload, Race race, CancellationToken cancellationToken)
    {
      IEnumerable<Language>? languages = await GetLanguagesAsync(payload, cancellationToken);
      race.Languages.Clear();
      if (languages != null)
      {
        race.Languages.AddRange(languages);
      }

      IEnumerable<RacialTrait>? traits = GetTraits(payload, race);
      race.Traits.Clear();
      if (traits != null)
      {
        race.Traits.AddRange(traits);
      }
    }
  }
}
