using MediatR;
using SkillCraft.Core.Customizations.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Customizations.Queries
{
  public class GetCustomizationsQuery : IRequest<ListModel<CustomizationModel>>
  {
    public bool? Deleted { get; set; }
    public string? Search { get; set; }
    public CustomizationType? Type { get; set; }

    public CustomizationSort? Sort { get; set; }
    public bool Desc { get; set; }

    public int? Index { get; set; }
    public int? Count { get; set; }
  }
}
