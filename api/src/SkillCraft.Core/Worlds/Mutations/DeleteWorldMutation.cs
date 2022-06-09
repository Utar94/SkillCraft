using MediatR;
using SkillCraft.Core.Worlds.Models;

namespace SkillCraft.Core.Worlds.Mutations
{
  public class DeleteWorldMutation : IRequest<WorldModel>
  {
    public DeleteWorldMutation(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
