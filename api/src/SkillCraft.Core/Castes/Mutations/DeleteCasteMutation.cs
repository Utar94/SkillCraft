using MediatR;
using SkillCraft.Core.Castes.Models;

namespace SkillCraft.Core.Castes.Mutations
{
  public class DeleteCasteMutation : IRequest<CasteModel>
  {
    public DeleteCasteMutation(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
