using MediatR;
using SkillCraft.Core.Languages.Models;

namespace SkillCraft.Core.Languages.Queries
{
  public class GetLanguageQuery : IRequest<LanguageModel?>
  {
    public GetLanguageQuery(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
