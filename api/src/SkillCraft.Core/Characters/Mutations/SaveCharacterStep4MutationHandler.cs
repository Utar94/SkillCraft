using AutoMapper;
using Logitar;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Castes;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Characters.Payloads;
using SkillCraft.Core.Educations;
using SkillCraft.Core.Talents;

namespace SkillCraft.Core.Characters.Mutations
{
  internal class SaveCharacterStep4MutationHandler : IRequestHandler<SaveCharacterStep4Mutation, CharacterModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public SaveCharacterStep4MutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<CharacterModel> Handle(SaveCharacterStep4Mutation request, CancellationToken cancellationToken)
    {
      SaveCharacterStep4Payload payload = request.Payload;

      Character character = await _dbContext.Characters
        .Include(x => x.Aspect1)
        .Include(x => x.Aspect2)
        .Include(x => x.Caste)
        .Include(x => x.Education)
        .Include(x => x.Nature)
        .Include(x => x.Race)
        .Include(x => x.Conditions).ThenInclude(x => x.Condition)
        .Include(x => x.Customizations)
        .Include(x => x.Languages)
        .Include(x => x.Talents).ThenInclude(x => x.Talent).ThenInclude(x => x!.Options)
        .Include(x => x.Talents).ThenInclude(x => x.Option)
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Character>(request.Id);

      if (character.WorldId != _appContext.World.Id || character.Creation?.Step == null)
      {
        throw new UnauthorizedOperationException<Character>(character, _appContext.UserId, _appContext.World);
      }

      Caste caste = await _dbContext.Castes
        .SingleOrDefaultAsync(x => x.Uuid == payload.CasteId, cancellationToken)
        ?? throw new EntityNotFoundException<Caste>(payload.CasteId, nameof(payload.CasteId));
      Education education = await _dbContext.Educations
        .SingleOrDefaultAsync(x => x.Uuid == payload.EducationId, cancellationToken)
        ?? throw new EntityNotFoundException<Caste>(payload.EducationId, nameof(payload.EducationId));

      if (caste.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Caste>(caste, _appContext.UserId, _appContext.World);
      }
      if (education.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Education>(education, _appContext.UserId, _appContext.World);
      }

      character.Caste = caste;
      character.CasteId = caste.Id;
      character.Education = education;
      character.EducationId = education.Id;

      character.Description = payload.Description?.CleanTrim();

      await UpdateTalentsAsync(character, payload, cancellationToken);
      UpdateSkillRanks(character, payload);
      character.Validate();

      character.Creation.Step = 4;

      await _dbContext.SaveChangesAsync(cancellationToken);

      _appContext.SetEntity(character);

      return _mapper.Map<CharacterModel>(character);
    }

    private static void UpdateSkillRanks(Character character, SaveCharacterStep4Payload payload)
    {
      if (payload.SkillRanks == null)
      {
        character.SkillRanks.Clear();
      }
      else
      {
        HashSet<Guid> ids = payload.SkillRanks
          .Where(x => x.Id.HasValue).Select(x => x.Id!.Value)
          .ToHashSet();
        character.SkillRanks = character.SkillRanks
          .Where(x => ids.Contains(x.Id))
          .ToList();

        Dictionary<Skill, int> newRanks = payload.SkillRanks.Where(x => x.Skill.HasValue)
          .GroupBy(x => x.Skill!.Value)
          .ToDictionary(x => x.Key, x => x.Count());
        HashSet<Skill> trained = character.Talents.Where(x => x.Talent?.Skill.HasValue == true)
          .Select(x => x.Talent!.Skill!.Value)
          .ToHashSet();
        foreach (var (skill, count) in newRanks)
        {
          character.SkillRanks.Add(new SkillRank(skill, trained.Contains(skill)));
        }
      }
    }

    private async Task UpdateTalentsAsync(Character character, SaveCharacterStep4Payload payload, CancellationToken cancellationToken)
    {
      Dictionary<Guid, CharacterTalent> characterTalents = character.Talents
        .ToDictionary(x => x.Uuid, x => x);

      character.Talents.Clear();

      if (payload.Talents != null)
      {
        var missingIds = new List<Guid>(capacity: payload.Talents.Count());
        var missingTalents = new List<Guid>(capacity: payload.Talents.Count());
        var missingOptions = new List<Guid>(capacity: payload.Talents.Count());

        HashSet<Guid> talentIds = payload.Talents.Where(x => x.TalentId.HasValue)
          .Select(x => x.TalentId!.Value)
          .ToHashSet();
        Dictionary<Guid, Talent> talents = await _dbContext.Talents
          .Include(x => x.Options)
          .Where(x => talentIds.Contains(x.Uuid))
          .ToDictionaryAsync(x => x.Uuid, x => x, cancellationToken);

        foreach (CharacterTalentPayload talentPayload in payload.Talents)
        {
          CharacterTalent? characterTalent;
          Talent? talent;
          if (talentPayload.Id.HasValue)
          {
            if (characterTalents.TryGetValue(talentPayload.Id.Value, out characterTalent))
            {
              talent = characterTalent.Talent ?? throw new InvalidOperationException($"The {nameof(characterTalent.Talent)} is required.");
            }
            else
            {
              missingIds.Add(talentPayload.Id.Value);

              continue;
            }
          }
          else if (talentPayload.TalentId.HasValue)
          {
            if (talents.TryGetValue(talentPayload.TalentId.Value, out talent))
            {
              if (talent.WorldId != _appContext.World.Id)
              {
                throw new UnauthorizedOperationException<Talent>(talent, _appContext.UserId, _appContext.World);
              }
            }
            else
            {
              missingTalents.Add(talentPayload.TalentId.Value);

              continue;
            }

            characterTalent = new CharacterTalent(character, talent);
          }
          else
          {
            throw new InvalidOperationException("The character talent payload is missing an ID.");
          }

          if (talent.Options.Any() && talentPayload.OptionId == null)
          {
            throw new TalentOptionRequiredException();
          }

          TalentOption? option = null;
          if (talentPayload.OptionId.HasValue)
          {
            option = talent.Options.SingleOrDefault(x => x.Uuid == talentPayload.OptionId.Value);
            if (option == null)
            {
              missingOptions.Add(talentPayload.OptionId.Value);

              continue;
            }
          }

          if (talentPayload.Cost > talent.Cost)
          {
            throw new TalentCostExceededException(talent, talentPayload.Cost);
          }

          characterTalent.Cost = talentPayload.Cost;
          characterTalent.Description = talentPayload.Description?.CleanTrim();

          characterTalent.Option = option;
          characterTalent.OptionId = option?.Id;

          character.Talents.Add(characterTalent);
        }
      }
    }
  }
}
