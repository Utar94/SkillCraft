using MediatR;
using SkillCraft.Core.Conditions.Models;
using SkillCraft.Core.Conditions.Payloads;

namespace SkillCraft.Core.Conditions.Mutations
{
  public class CreateConditionMutation : IRequest<ConditionModel>
  {
    public CreateConditionMutation(CreateConditionPayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateConditionPayload Payload { get; }
  }
}
