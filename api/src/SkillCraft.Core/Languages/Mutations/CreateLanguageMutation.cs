using MediatR;
using SkillCraft.Core.Languages.Models;
using SkillCraft.Core.Languages.Payloads;

namespace SkillCraft.Core.Languages.Mutations
{
  public class CreateLanguageMutation : IRequest<LanguageModel>
  {
    public CreateLanguageMutation(CreateLanguagePayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateLanguagePayload Payload { get; }
  }
}
