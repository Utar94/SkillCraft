using MediatR;
using SkillCraft.Core.Characters.Models;

namespace SkillCraft.Core.Characters.Mutations
{
  public class DeleteCharacterMutation : IRequest<CharacterModel>
  {
    public DeleteCharacterMutation(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
