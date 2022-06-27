using MediatR;
using SkillCraft.Core.Characters.Models;

namespace SkillCraft.Core.Characters.Queries
{
  public class GetCharacterQuery : IRequest<CharacterModel?>
  {
    public GetCharacterQuery(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
