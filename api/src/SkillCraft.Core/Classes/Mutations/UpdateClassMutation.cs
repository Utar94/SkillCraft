using MediatR;
using SkillCraft.Core.Classes.Models;
using SkillCraft.Core.Classes.Payloads;

namespace SkillCraft.Core.Classes.Mutations
{
  public class UpdateClassMutation : IRequest<ClassModel>
  {
    public UpdateClassMutation(Guid id, UpdateClassPayload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public UpdateClassPayload Payload { get; }
  }
}
