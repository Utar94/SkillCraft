using MediatR;
using SkillCraft.Core.Worlds.Models;

namespace SkillCraft.Core.Worlds.Queries
{
  public class GetWorldQuery : IRequest<WorldModel?>
  {
    public GetWorldQuery(string id)
    {
      Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    public string Id { get; }
  }
}
