using MediatR;
using SkillCraft.Core.Models;
using SkillCraft.Core.Talents.Models;

namespace SkillCraft.Core.Talents.Queries
{
  public class GetTalentsQuery : IRequest<ListModel<TalentModel>>
  {
    public bool? Deleted { get; set; }
    public bool? MultipleAcquisition { get; set; }
    public string? Search { get; set; }
    public IEnumerable<int>? Tiers { get; set; }

    public TalentSort? Sort { get; set; }
    public bool Desc { get; set; }

    public int? Index { get; set; }
    public int? Count { get; set; }
  }
}
