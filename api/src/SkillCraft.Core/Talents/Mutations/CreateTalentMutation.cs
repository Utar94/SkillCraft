using MediatR;
using SkillCraft.Core.Talents.Models;
using SkillCraft.Core.Talents.Payloads;

namespace SkillCraft.Core.Talents.Mutations
{
  public class CreateTalentMutation : IRequest<TalentModel>
  {
    public CreateTalentMutation(CreateTalentPayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateTalentPayload Payload { get; }
  }
}
