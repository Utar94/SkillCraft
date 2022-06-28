using AutoMapper;
using MediatR;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Repositories;

namespace SkillCraft.Core.Characters.Queries
{
  internal class GetCharacterQueryHandler : IRequestHandler<GetCharacterQuery, CharacterModel?>
  {
    private readonly IApplicationContext _appContext;
    private readonly ICharacterRepository _characterRepository;
    private readonly IMapper _mapper;

    public GetCharacterQueryHandler(
      IApplicationContext appContext,
      ICharacterRepository characterRepository,
      IMapper mapper
    )
    {
      _appContext = appContext;
      _characterRepository = characterRepository;
      _mapper = mapper;
    }

    public async Task<CharacterModel?> Handle(GetCharacterQuery request, CancellationToken cancellationToken)
    {
      Character character = await _characterRepository
        .GetAsync(request.Id, readOnly: true, cancellationToken)
        ?? throw new EntityNotFoundException<Character>(request.Id);

      if (character == null)
      {
        return null;
      }
      else if (character.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Character>(character, _appContext.UserId, _appContext.World);
      }

      return _mapper.Map<CharacterModel>(character);
    }
  }
}
