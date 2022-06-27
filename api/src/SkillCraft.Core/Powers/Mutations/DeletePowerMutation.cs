using MediatR;
using SkillCraft.Core.Powers.Models;

namespace SkillCraft.Core.Powers.Mutations
{
  public class DeletePowerMutation : IRequest<PowerModel>
  {
    public DeletePowerMutation(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
