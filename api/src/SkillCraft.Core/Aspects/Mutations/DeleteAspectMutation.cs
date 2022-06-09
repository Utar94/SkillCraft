using MediatR;
using SkillCraft.Core.Aspects.Models;

namespace SkillCraft.Core.Aspects.Mutations
{
  public class DeleteAspectMutation : IRequest<AspectModel>
  {
    public DeleteAspectMutation(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
