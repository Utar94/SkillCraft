using MediatR;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Characters.Payloads;

namespace SkillCraft.Core.Characters.Mutations
{
  public class SaveCharacterStep2Mutation : IRequest<CharacterModel>
  {
    public SaveCharacterStep2Mutation(SaveCharacterStep2Payload payload, Guid? id = null)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid? Id { get; }
    public SaveCharacterStep2Payload Payload { get; }
  }
}
