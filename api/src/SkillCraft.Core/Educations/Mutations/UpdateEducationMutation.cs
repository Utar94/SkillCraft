using MediatR;
using SkillCraft.Core.Educations.Models;
using SkillCraft.Core.Educations.Payloads;

namespace SkillCraft.Core.Educations.Mutations
{
  public class UpdateEducationMutation : IRequest<EducationModel>
  {
    public UpdateEducationMutation(Guid id, UpdateEducationPayload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public UpdateEducationPayload Payload { get; }
  }
}
