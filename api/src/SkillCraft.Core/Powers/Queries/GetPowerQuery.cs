using MediatR;
using SkillCraft.Core.Powers.Models;

namespace SkillCraft.Core.Powers.Queries
{
  public class GetPowerQuery : IRequest<PowerModel?>
  {
    public GetPowerQuery(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
