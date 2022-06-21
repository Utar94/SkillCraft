using MediatR;
using SkillCraft.Core.Talents.Models;

namespace SkillCraft.Core.Talents.Queries
{
  public class GetTalentQuery : IRequest<TalentModel?>
  {
    public GetTalentQuery(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
