using MediatR;
using SkillCraft.Core.Customizations.Models;

namespace SkillCraft.Core.Customizations.Mutations
{
  public class DeleteCustomizationMutation : IRequest<CustomizationModel>
  {
    public DeleteCustomizationMutation(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
