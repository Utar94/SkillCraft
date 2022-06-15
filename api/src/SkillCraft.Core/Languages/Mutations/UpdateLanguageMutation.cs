using MediatR;
using SkillCraft.Core.Languages.Models;
using SkillCraft.Core.Languages.Payloads;

namespace SkillCraft.Core.Languages.Mutations
{
  public class UpdateLanguageMutation : IRequest<LanguageModel>
  {
    public UpdateLanguageMutation(Guid id, UpdateLanguagePayload payload)
    {
      Id = id;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public UpdateLanguagePayload Payload { get; }
  }
}
