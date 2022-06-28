using MediatR;
using SkillCraft.Core.Classes.Models;

namespace SkillCraft.Core.Classes.Queries
{
  public class GetClassQuery : IRequest<ClassModel?>
  {
    public GetClassQuery(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
