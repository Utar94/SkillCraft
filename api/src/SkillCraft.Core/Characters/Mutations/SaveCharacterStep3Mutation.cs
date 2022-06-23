using MediatR;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Characters.Payloads;

namespace SkillCraft.Core.Characters.Mutations
{
  public class SaveCharacterStep3Mutation : IRequest<CharacterModel>
  {
    public SaveCharacterStep3Mutation(Guid id, SaveCharacterStep3Payload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public SaveCharacterStep3Payload Payload { get; }
  }
}
