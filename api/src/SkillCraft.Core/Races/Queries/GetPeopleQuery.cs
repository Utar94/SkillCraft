using MediatR;
using SkillCraft.Core.Races.Models;

namespace SkillCraft.Core.Races.Queries
{
  public class GetPeopleQuery : IRequest<IEnumerable<RaceModel>>
  {
    public GetPeopleQuery(Guid raceId)
    {
      RaceId = raceId;
    }

    public Guid RaceId { get; }
  }
}
