using MediatR;
using SkillCraft.Core.Models;
using SkillCraft.Core.Natures.Models;

namespace SkillCraft.Core.Natures.Queries
{
  public class GetNaturesQuery : IRequest<ListModel<NatureModel>>
  {
    public bool? Deleted { get; set; }
    public string? Search { get; set; }

    public NatureSort? Sort { get; set; }
    public bool Desc { get; set; }

    public int? Index { get; set; }
    public int? Count { get; set; }
  }
}
