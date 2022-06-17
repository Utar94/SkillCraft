using MediatR;
using SkillCraft.Core.Races.Models;

namespace SkillCraft.Core.Races.Queries
{
  public class GetRaceQuery : IRequest<RaceModel?>
  {
    public GetRaceQuery(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
