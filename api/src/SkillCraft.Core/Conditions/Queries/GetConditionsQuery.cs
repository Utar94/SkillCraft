using MediatR;
using SkillCraft.Core.Conditions.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Conditions.Queries
{
  public class GetConditionsQuery : IRequest<ListModel<ConditionModel>>
  {
    public bool? Deleted { get; set; }
    public string? Search { get; set; }

    public ConditionSort? Sort { get; set; }
    public bool Desc { get; set; }

    public int? Index { get; set; }
    public int? Count { get; set; }
  }
}
