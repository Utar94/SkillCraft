using MediatR;
using SkillCraft.Core.Models;
using SkillCraft.Core.Worlds.Models;

namespace SkillCraft.Core.Worlds.Queries
{
  public class GetWorldsQuery : IRequest<ListModel<WorldModel>>
  {
    public string? Search { get; set; }

    public WorldSort? Sort { get; set; }
    public bool Desc { get; set; }

    public int? Index { get; set; }
    public int? Count { get; set; }
  }
}
