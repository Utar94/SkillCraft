using MediatR;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Characters.Payloads;

namespace SkillCraft.Core.Characters.Mutations
{
  public class SaveCharacterStep4Mutation : IRequest<CharacterModel>
  {
    public SaveCharacterStep4Mutation(Guid id, SaveCharacterStep4Payload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public SaveCharacterStep4Payload Payload { get; }
  }
}
