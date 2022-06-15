using MediatR;
using SkillCraft.Core.Castes.Models;
using SkillCraft.Core.Castes.Payloads;

namespace SkillCraft.Core.Castes.Mutations
{
  public class UpdateCasteMutation : IRequest<CasteModel>
  {
    public UpdateCasteMutation(Guid id, UpdateCastePayload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public UpdateCastePayload Payload { get; }
  }
}
