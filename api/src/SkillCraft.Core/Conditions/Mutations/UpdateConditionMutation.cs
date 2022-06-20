using MediatR;
using SkillCraft.Core.Conditions.Models;
using SkillCraft.Core.Conditions.Payloads;

namespace SkillCraft.Core.Conditions.Mutations
{
  public class UpdateConditionMutation : IRequest<ConditionModel>
  {
    public UpdateConditionMutation(Guid id, UpdateConditionPayload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public UpdateConditionPayload Payload { get; }
  }
}
