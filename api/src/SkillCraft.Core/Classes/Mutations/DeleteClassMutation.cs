using MediatR;
using SkillCraft.Core.Classes.Models;

namespace SkillCraft.Core.Classes.Mutations
{
  public class DeleteClassMutation : IRequest<ClassModel>
  {
    public DeleteClassMutation(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
