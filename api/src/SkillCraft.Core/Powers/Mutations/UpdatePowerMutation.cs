using MediatR;
using SkillCraft.Core.Powers.Models;
using SkillCraft.Core.Powers.Payloads;

namespace SkillCraft.Core.Powers.Mutations
{
  public class UpdatePowerMutation : IRequest<PowerModel>
  {
    public UpdatePowerMutation(Guid id, UpdatePowerPayload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public UpdatePowerPayload Payload { get; }
  }
}
