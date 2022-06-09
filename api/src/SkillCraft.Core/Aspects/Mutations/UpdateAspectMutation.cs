using MediatR;
using SkillCraft.Core.Aspects.Models;
using SkillCraft.Core.Aspects.Payloads;

namespace SkillCraft.Core.Aspects.Mutations
{
  public class UpdateAspectMutation : IRequest<AspectModel>
  {
    public UpdateAspectMutation(Guid id, UpdateAspectPayload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public UpdateAspectPayload Payload { get; }
  }
}
