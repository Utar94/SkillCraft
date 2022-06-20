using MediatR;
using SkillCraft.Core.Models;
using SkillCraft.Core.Races.Models;

namespace SkillCraft.Core.Races.Queries
{
  public class GetRacesQuery : IRequest<ListModel<RaceModel>>
  {
    public bool? Deleted { get; set; }
    public string? Search { get; set; }

    public RaceSort? Sort { get; set; }
    public bool Desc { get; set; }

    public int? Index { get; set; }
    public int? Count { get; set; }
  }
}
