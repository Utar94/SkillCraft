using MediatR;
using SkillCraft.Core.Educations.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Educations.Queries
{
  public class GetEducationsQuery : IRequest<ListModel<EducationModel>>
  {
    public bool? Deleted { get; set; }
    public string? Search { get; set; }

    public EducationSort? Sort { get; set; }
    public bool Desc { get; set; }

    public int? Index { get; set; }
    public int? Count { get; set; }
  }
}
