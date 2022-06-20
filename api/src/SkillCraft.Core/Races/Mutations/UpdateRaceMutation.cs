using MediatR;
using SkillCraft.Core.Races.Models;
using SkillCraft.Core.Races.Payloads;

namespace SkillCraft.Core.Races.Mutations
{
  public class UpdateRaceMutation : IRequest<RaceModel>
  {
    public UpdateRaceMutation(Guid id, UpdateRacePayload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public UpdateRacePayload Payload { get; }
  }
}
