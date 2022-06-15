using MediatR;
using SkillCraft.Core.Languages.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Languages.Queries
{
  public class GetLanguagesQuery : IRequest<ListModel<LanguageModel>>
  {
    public bool? Deleted { get; set; }
    public string? Search { get; set; }

    public LanguageSort? Sort { get; set; }
    public bool Desc { get; set; }

    public int? Index { get; set; }
    public int? Count { get; set; }
  }
}
