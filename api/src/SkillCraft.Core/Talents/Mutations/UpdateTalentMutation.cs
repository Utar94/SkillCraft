using MediatR;
using SkillCraft.Core.Talents.Models;
using SkillCraft.Core.Talents.Payloads;

namespace SkillCraft.Core.Talents.Mutations
{
  public class UpdateTalentMutation : IRequest<TalentModel>
  {
    public UpdateTalentMutation(Guid id, UpdateTalentPayload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public UpdateTalentPayload Payload { get; }
  }
}
