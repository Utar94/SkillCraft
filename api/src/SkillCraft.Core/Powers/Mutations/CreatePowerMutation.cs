using MediatR;
using SkillCraft.Core.Powers.Models;
using SkillCraft.Core.Powers.Payloads;

namespace SkillCraft.Core.Powers.Mutations
{
  public class CreatePowerMutation : IRequest<PowerModel>
  {
    public CreatePowerMutation(CreatePowerPayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreatePowerPayload Payload { get; }
  }
}
