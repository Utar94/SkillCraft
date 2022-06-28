using MediatR;
using SkillCraft.Core.Classes.Models;
using SkillCraft.Core.Classes.Payloads;

namespace SkillCraft.Core.Classes.Mutations
{
  public class CreateClassMutation : IRequest<ClassModel>
  {
    public CreateClassMutation(CreateClassPayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateClassPayload Payload { get; }
  }
}
