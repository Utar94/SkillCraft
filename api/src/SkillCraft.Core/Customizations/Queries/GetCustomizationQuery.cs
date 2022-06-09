using MediatR;
using SkillCraft.Core.Customizations.Models;

namespace SkillCraft.Core.Customizations.Queries
{
  public class GetCustomizationQuery : IRequest<CustomizationModel?>
  {
    public GetCustomizationQuery(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
