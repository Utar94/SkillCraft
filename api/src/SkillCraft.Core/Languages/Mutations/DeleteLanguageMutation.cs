using MediatR;
using SkillCraft.Core.Languages.Models;

namespace SkillCraft.Core.Languages.Mutations
{
  public class DeleteLanguageMutation : IRequest<LanguageModel>
  {
    public DeleteLanguageMutation(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
