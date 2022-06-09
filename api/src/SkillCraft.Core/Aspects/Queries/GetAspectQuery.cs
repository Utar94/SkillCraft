using MediatR;
using SkillCraft.Core.Aspects.Models;

namespace SkillCraft.Core.Aspects.Queries
{
  public class GetAspectQuery : IRequest<AspectModel?>
  {
    public GetAspectQuery(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
