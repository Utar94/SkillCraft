using AutoMapper;
using MediatR;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Repositories;

namespace SkillCraft.Core.Characters.Mutations
{
  internal class DeleteCharacterMutationHandler : IRequestHandler<DeleteCharacterMutation, CharacterModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly ICharacterRepository _characterRepository;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public DeleteCharacterMutationHandler(
      IApplicationContext appContext,
      ICharacterRepository characterRepository,
      IDbContext dbContext,
      IMapper mapper
    )
    {
      _appContext = appContext;
      _characterRepository = characterRepository;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<CharacterModel> Handle(DeleteCharacterMutation request, CancellationToken cancellationToken)
    {
      Character character = await _characterRepository
        .GetAsync(request.Id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Character>(request.Id);

      if (character.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Character>(character, _appContext.UserId, _appContext.World);
      }

      character.Delete(_appContext.UserId);

      await _dbContext.SaveChangesAsync(cancellationToken);

      return _mapper.Map<CharacterModel>(character);
    }
  }
}
