using MediatR;
using SkillCraft.Core.Conditions.Models;

namespace SkillCraft.Core.Conditions.Mutations
{
  public class DeleteConditionMutation : IRequest<ConditionModel>
  {
    public DeleteConditionMutation(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
