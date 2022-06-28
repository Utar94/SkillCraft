using MediatR;
using SkillCraft.Core.Classes.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Classes.Queries
{
  public class GetClassesQuery : IRequest<ListModel<ClassModel>>
  {
    public bool? Deleted { get; set; }
    public string? Search { get; set; }
    public IEnumerable<int>? Tiers { get; set; }

    public ClassSort? Sort { get; set; }
    public bool Desc { get; set; }

    public int? Index { get; set; }
    public int? Count { get; set; }
  }
}
