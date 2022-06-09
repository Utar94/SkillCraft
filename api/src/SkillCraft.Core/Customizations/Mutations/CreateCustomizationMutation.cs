using MediatR;
using SkillCraft.Core.Customizations.Models;
using SkillCraft.Core.Customizations.Payloads;

namespace SkillCraft.Core.Customizations.Mutations
{
  public class CreateCustomizationMutation : IRequest<CustomizationModel>
  {
    public CreateCustomizationMutation(CreateCustomizationPayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateCustomizationPayload Payload { get; }
  }
}
