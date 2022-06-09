using MediatR;
using SkillCraft.Core.Customizations.Models;
using SkillCraft.Core.Customizations.Payloads;

namespace SkillCraft.Core.Customizations.Mutations
{
  public class UpdateCustomizationMutation : IRequest<CustomizationModel>
  {
    public UpdateCustomizationMutation(Guid id, UpdateCustomizationPayload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public UpdateCustomizationPayload Payload { get; }
  }
}
