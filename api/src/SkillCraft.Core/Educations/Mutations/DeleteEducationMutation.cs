using MediatR;
using SkillCraft.Core.Educations.Models;

namespace SkillCraft.Core.Educations.Mutations
{
  public class DeleteEducationMutation : IRequest<EducationModel>
  {
    public DeleteEducationMutation(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
