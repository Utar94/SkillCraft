using MediatR;
using SkillCraft.Core.Educations.Models;

namespace SkillCraft.Core.Educations.Queries
{
  public class GetEducationQuery : IRequest<EducationModel?>
  {
    public GetEducationQuery(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
