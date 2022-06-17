using MediatR;
using SkillCraft.Core.Races.Models;

namespace SkillCraft.Core.Races.Mutations
{
  public class DeleteRaceMutation : IRequest<RaceModel>
  {
    public DeleteRaceMutation(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
