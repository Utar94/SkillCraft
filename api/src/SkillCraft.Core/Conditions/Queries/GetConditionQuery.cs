using MediatR;
using SkillCraft.Core.Conditions.Models;

namespace SkillCraft.Core.Conditions.Queries
{
  public class GetConditionQuery : IRequest<ConditionModel?>
  {
    public GetConditionQuery(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
