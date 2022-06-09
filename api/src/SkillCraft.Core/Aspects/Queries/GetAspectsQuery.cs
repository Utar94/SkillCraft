using MediatR;
using SkillCraft.Core.Aspects.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Aspects.Queries
{
  public class GetAspectsQuery : IRequest<ListModel<AspectModel>>
  {
    public bool? Deleted { get; set; }
    public string? Search { get; set; }

    public AspectSort? Sort { get; set; }
    public bool Desc { get; set; }

    public int? Index { get; set; }
    public int? Count { get; set; }
  }
}
