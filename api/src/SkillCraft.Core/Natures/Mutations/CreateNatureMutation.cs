using MediatR;
using SkillCraft.Core.Natures.Models;
using SkillCraft.Core.Natures.Payloads;

namespace SkillCraft.Core.Natures.Mutations
{
  public class CreateNatureMutation : IRequest<NatureModel>
  {
    public CreateNatureMutation(CreateNaturePayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateNaturePayload Payload { get; }
  }
}
