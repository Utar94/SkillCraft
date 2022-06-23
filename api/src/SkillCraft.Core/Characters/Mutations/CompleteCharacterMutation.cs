using MediatR;
using SkillCraft.Core.Characters.Models;

namespace SkillCraft.Core.Characters.Mutations
{
  public class CompleteCharacterMutation : IRequest<CharacterModel>
  {
    public CompleteCharacterMutation(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
