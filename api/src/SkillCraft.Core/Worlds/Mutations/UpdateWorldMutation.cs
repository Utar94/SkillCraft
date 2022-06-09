using MediatR;
using SkillCraft.Core.Worlds.Models;
using SkillCraft.Core.Worlds.Payloads;

namespace SkillCraft.Core.Worlds.Mutations
{
  public class UpdateWorldMutation : IRequest<WorldModel>
  {
    public UpdateWorldMutation(Guid id, UpdateWorldPayload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public UpdateWorldPayload Payload { get; }
  }
}
