using AutoMapper;
using Logitar;
using MediatR;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Characters.Payloads;
using SkillCraft.Core.Repositories;

namespace SkillCraft.Core.Characters.Mutations
{
  internal class UpdateCharacterMutationHandler : IRequestHandler<UpdateCharacterMutation, CharacterModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly ICharacterRepository _characterRepository;
    private readonly ICharacterService _characterService;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public UpdateCharacterMutationHandler(
      IApplicationContext appContext,
      ICharacterRepository characterRepository,
      ICharacterService characterService,
      IDbContext dbContext,
      IMapper mapper
    )
    {
      _appContext = appContext;
      _characterRepository = characterRepository;
      _characterService = characterService;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<CharacterModel> Handle(UpdateCharacterMutation request, CancellationToken cancellationToken)
    {
      UpdateCharacterPayload payload = request.Payload;

      Character character = await _characterRepository
        .GetAsync(request.Id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Character>(request.Id);

      if (character.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Character>(character, _appContext.UserId, _appContext.World);
      }

      character.Name = payload.Name.Trim();
      character.Player = payload.Player?.CleanTrim();

      character.Stature = payload.Stature;
      character.Weight = payload.Weight;
      character.Age = payload.Age;

      character.Experience = payload.Experience;
      character.Vitality = payload.Vitality;
      character.Stamina = payload.Stamina;

      character.BloodAlcoholContent = payload.BloodAlcoholContent;
      character.Intoxication = payload.Intoxication;

      character.Description = payload.Description?.CleanTrim();

      _characterService.UpdateBonuses(character, payload.Bonuses);

      await _characterService.UpdateConditionsAsync(character, payload.Conditions, cancellationToken);
      await _characterService.UpdateLanguagesAsync(character, payload.LanguageIds?.ToHashSet(), cancellationToken);

      await _characterService.UpdatePowersAsync(character, payload.Powers, cancellationToken);
      await _characterService.UpdateTalentsAsync(character, payload.Talents, cancellationToken);
      _characterService.UpdateSkillRanks(character, payload.SkillRanks);

      character.Update(_appContext.UserId);

      await _dbContext.SaveChangesAsync(cancellationToken);

      _appContext.SetEntity(character);

      return _mapper.Map<CharacterModel>(character);
    }
  }
}
