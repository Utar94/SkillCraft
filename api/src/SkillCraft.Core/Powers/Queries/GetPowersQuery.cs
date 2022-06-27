using MediatR;
using SkillCraft.Core.Models;
using SkillCraft.Core.Powers.Models;

namespace SkillCraft.Core.Powers.Queries
{
  public class GetPowersQuery : IRequest<ListModel<PowerModel>>
  {
    public bool? Deleted { get; set; }
    public string? Search { get; set; }
    public IEnumerable<int>? Tiers { get; set; }

    public PowerSort? Sort { get; set; }
    public bool Desc { get; set; }

    public int? Index { get; set; }
    public int? Count { get; set; }
  }
}
