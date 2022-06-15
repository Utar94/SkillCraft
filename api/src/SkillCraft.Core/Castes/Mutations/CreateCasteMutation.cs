using MediatR;
using SkillCraft.Core.Castes.Models;
using SkillCraft.Core.Castes.Payloads;

namespace SkillCraft.Core.Castes.Mutations
{
  public class CreateCasteMutation : IRequest<CasteModel>
  {
    public CreateCasteMutation(CreateCastePayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateCastePayload Payload { get; }
  }
}
