using MediatR;
using SkillCraft.Core.Aspects.Models;
using SkillCraft.Core.Aspects.Payloads;

namespace SkillCraft.Core.Aspects.Mutations
{
  public class CreateAspectMutation : IRequest<AspectModel>
  {
    public CreateAspectMutation(CreateAspectPayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateAspectPayload Payload { get; }
  }
}
