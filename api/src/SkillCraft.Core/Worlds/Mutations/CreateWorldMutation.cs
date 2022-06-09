using MediatR;
using SkillCraft.Core.Worlds.Models;
using SkillCraft.Core.Worlds.Payloads;

namespace SkillCraft.Core.Worlds.Mutations
{
  public class CreateWorldMutation : IRequest<WorldModel>
  {
    public CreateWorldMutation(CreateWorldPayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateWorldPayload Payload { get; }
  }
}
