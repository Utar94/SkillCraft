using MediatR;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Characters.Payloads;

namespace SkillCraft.Core.Characters.Mutations
{
  public class LevelUpCharacterMutation : IRequest<CharacterModel>
  {
    public LevelUpCharacterMutation(Guid id, LevelUpCharacterPayload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public LevelUpCharacterPayload Payload { get; }
  }
}
