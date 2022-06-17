using MediatR;
using SkillCraft.Core.Races.Models;
using SkillCraft.Core.Races.Payloads;

namespace SkillCraft.Core.Races.Mutations
{
  public class CreateRaceMutation : IRequest<RaceModel>
  {
    public CreateRaceMutation(CreateRacePayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateRacePayload Payload { get; }
  }
}
