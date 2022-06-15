using MediatR;
using SkillCraft.Core.Castes.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Castes.Queries
{
  public class GetCastesQuery : IRequest<ListModel<CasteModel>>
  {
    public bool? Deleted { get; set; }
    public string? Search { get; set; }

    public CasteSort? Sort { get; set; }
    public bool Desc { get; set; }

    public int? Index { get; set; }
    public int? Count { get; set; }
  }
}
