using AutoMapper;
using Logitar;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Castes;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Characters.Payloads;
using SkillCraft.Core.Educations;
using SkillCraft.Core.Repositories;

namespace SkillCraft.Core.Characters.Mutations
{
  internal class SaveCharacterStep4MutationHandler : IRequestHandler<SaveCharacterStep4Mutation, CharacterModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly ICharacterRepository _characterRepository;
    private readonly ICharacterService _characterService;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public SaveCharacterStep4MutationHandler(
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

    public async Task<CharacterModel> Handle(SaveCharacterStep4Mutation request, CancellationToken cancellationToken)
    {
      SaveCharacterStep4Payload payload = request.Payload;

      Character character = await _characterRepository
        .GetAsync(request.Id, readOnly: false, cancellationToken)
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

      await _characterService.UpdatePowersAsync(character, payload.Powers, cancellationToken);
      await _characterService.UpdateTalentsAsync(character, payload.Talents, cancellationToken);
      _characterService.UpdateSkillRanks(character, payload.SkillRanks);

      character.Creation.Step = 4;

      character.Update(_appContext.UserId);

      await _dbContext.SaveChangesAsync(cancellationToken);

      _appContext.SetEntity(character);

      return _mapper.Map<CharacterModel>(character);
    }
  }
}
