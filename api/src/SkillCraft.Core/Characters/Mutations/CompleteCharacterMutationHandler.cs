using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Characters.Models;

namespace SkillCraft.Core.Characters.Mutations
{
  internal class CompleteCharacterMutationHandler : IRequestHandler<CompleteCharacterMutation, CharacterModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public CompleteCharacterMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<CharacterModel> Handle(CompleteCharacterMutation request, CancellationToken cancellationToken)
    {
      Character character = await _dbContext.Characters
        .Include(x => x.Aspect1)
        .Include(x => x.Aspect2)
        .Include(x => x.Caste)
        .Include(x => x.Education)
        .Include(x => x.Nature)
        .Include(x => x.Race)
        .Include(x => x.Conditions)
        .Include(x => x.Customizations)
        .Include(x => x.Languages)
        .Include(x => x.Talents).ThenInclude(x => x.Talent)
        .Include(x => x.Talents).ThenInclude(x => x.Option)
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Character>(request.Id);

      if (character.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Character>(character, _appContext.UserId, _appContext.World);
      }
      else if (character.Creation?.Step == null)
      {
        throw new CharacterAlreadyCompletedException(character);
      }

      character.Creation.Step = null;
      character.Vitality = character.MaxVitality;
      character.Stamina = character.MaxStamina;

      character.Update(_appContext.UserId);

      await _dbContext.SaveChangesAsync(cancellationToken);

      _appContext.SetEntity(character);

      return _mapper.Map<CharacterModel>(character);
    }
  }
}
