using MediatR;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Characters.Payloads;

namespace SkillCraft.Core.Characters.Mutations
{
  public class UpdateCharacterMutation : IRequest<CharacterModel>
  {
    public UpdateCharacterMutation(Guid id, UpdateCharacterPayload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public UpdateCharacterPayload Payload { get; }
  }
}
