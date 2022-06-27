using MediatR;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Characters.Queries
{
  public class GetCharactersQuery : IRequest<ListModel<CharacterModel>>
  {
    public bool? Deleted { get; set; }
    public string? Search { get; set; }

    public CharacterSort? Sort { get; set; }
    public bool Desc { get; set; }

    public int? Index { get; set; }
    public int? Count { get; set; }
  }
}
