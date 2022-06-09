using MediatR;
using SkillCraft.Core.Natures.Models;
using SkillCraft.Core.Natures.Payloads;

namespace SkillCraft.Core.Natures.Mutations
{
  public class UpdateNatureMutation : IRequest<NatureModel>
  {
    public UpdateNatureMutation(Guid id, UpdateNaturePayload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public UpdateNaturePayload Payload { get; }
  }
}
