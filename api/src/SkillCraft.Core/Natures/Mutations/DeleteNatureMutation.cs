using MediatR;
using SkillCraft.Core.Natures.Models;

namespace SkillCraft.Core.Natures.Mutations
{
  public class DeleteNatureMutation : IRequest<NatureModel>
  {
    public DeleteNatureMutation(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
