using AutoMapper;
using FluentValidation;
using SkillCraft.Core.Talents.Models;
using SkillCraft.Core.Talents.Payload;

namespace SkillCraft.Core.Talents
{
  internal class TalentService : ITalentService
  {
    private readonly IMapper _mapper;
    private readonly ITalentQuerier _querier;
    private readonly IRepository<Talent> _repository;
    private readonly IUserContext _userContext;
    private readonly IValidator<Talent> _validator;

    public TalentService(
      IMapper mapper,
      ITalentQuerier querier,
      IRepository<Talent> repository,
      IUserContext userContext,
      IValidator<Talent> validator
    )
    {
      _mapper = mapper;
      _querier = querier;
      _repository = repository;
      _userContext = userContext;
      _validator = validator;
    }

    public async Task<TalentModel> CreateAsync(CreateTalentPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      Talent? requiredTalent = await GetRequiredTalentAsync(payload, cancellationToken);

      var talent = new Talent(payload, _userContext.Id, _userContext.World, requiredTalent);
      UpdateOptions(payload, talent);
      _validator.ValidateAndThrow(talent);

      await _repository.SaveAsync(talent, cancellationToken);

      return _mapper.Map<TalentModel>(talent);
    }

    public async Task<TalentModel> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Talent talent = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Talent>(id);

      if (talent.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Talent>(talent, _userContext.Id);
      }

      talent.Delete(_userContext.Id);
      await _repository.SaveAsync(talent, cancellationToken);

      return _mapper.Map<TalentModel>(talent);
    }

    public async Task<TalentModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Talent? talent = await _querier.GetAsync(id, readOnly: true, cancellationToken);
      if (talent == null)
      {
        return null;
      }
      else if (talent.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Talent>(talent, _userContext.Id);
      }

      return _mapper.Map<TalentModel>(talent);
    }

    public async Task<ListModel<TalentModel>> GetAsync(bool? multipleAcquisition, string? search, Skill? skill, IEnumerable<int>? tiers,
      TalentSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      PagedList<Talent> talents = await _querier.GetPagedAsync(_userContext.World.Sid, multipleAcquisition, search, skill, tiers,
        sort, desc,
        index, count,
        readOnly: true, cancellationToken);

      return ListModel<TalentModel>.From(talents, _mapper);
    }

    public async Task<TalentModel> UpdateAsync(Guid id, UpdateTalentPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      Talent? requiredTalent = await GetRequiredTalentAsync(payload, cancellationToken);

      Talent talent = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Talent>(id);

      if (talent.CreatedById != _userContext.Id)
      {
        throw new ForbiddenException<Talent>(talent, _userContext.Id);
      }

      talent.Update(payload, _userContext.Id, requiredTalent);
      UpdateOptions(payload, talent);
      await _repository.SaveAsync(talent, cancellationToken);

      return _mapper.Map<TalentModel>(talent);
    }

    private async Task<Talent?> GetRequiredTalentAsync(SaveTalentPayload payload, CancellationToken cancellationToken)
    {
      if (payload.RequiredTalentId.HasValue)
      {
        Talent requiredTalent = await _querier.GetAsync(payload.RequiredTalentId.Value, readOnly: false, cancellationToken)
          ?? throw new EntityNotFoundException<Talent>(payload.RequiredTalentId.Value, nameof(payload.RequiredTalentId));

        if (requiredTalent.WorldSid != _userContext.World.Sid)
        {
          throw new ForbiddenException<Talent>(requiredTalent, _userContext.Id);
        }

        return requiredTalent;
      }

      return null;
    }

    private static void UpdateOptions(SaveTalentPayload payload, Talent talent)
    {
      if (payload.Options == null)
      {
        talent.Options.Clear();
      }
      else
      {
        Dictionary<Guid, TalentOption> talentOptions = talent.Options.ToDictionary(x => x.Id, x => x) ?? new();

        talent.Options.Clear();

        IEnumerable<Guid> missingTraits = payload.Options.Where(x => x.Id.HasValue).Select(x => x.Id!.Value)
          .Except(talentOptions.Keys);
        if (missingTraits.Any())
        {
          throw new TalentOptionsNotFoundException(missingTraits, nameof(payload.Options));
        }

        foreach (TalentOptionPayload optionPayload in payload.Options)
        {
          if (optionPayload.Id.HasValue)
          {
            TalentOption trait = talentOptions[optionPayload.Id.Value];
            trait.Update(optionPayload.Name, optionPayload.Description);
            talent.Options.Add(trait);
          }
          else
          {
            talent.Options.Add(new TalentOption(talent, optionPayload.Name, optionPayload.Description));
          }
        }
      }
    }
  }
}
