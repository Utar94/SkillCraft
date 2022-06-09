using MediatR;
using SkillCraft.Core.Natures.Models;

namespace SkillCraft.Core.Natures.Queries
{
  public class GetNatureQuery : IRequest<NatureModel?>
  {
    public GetNatureQuery(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
