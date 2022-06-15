using MediatR;
using SkillCraft.Core.Castes.Models;

namespace SkillCraft.Core.Castes.Queries
{
  public class GetCasteQuery : IRequest<CasteModel?>
  {
    public GetCasteQuery(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
