using MediatR;
using SkillCraft.Core.Educations.Models;
using SkillCraft.Core.Educations.Payloads;

namespace SkillCraft.Core.Educations.Mutations
{
  public class CreateEducationMutation : IRequest<EducationModel>
  {
    public CreateEducationMutation(CreateEducationPayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateEducationPayload Payload { get; }
  }
}
