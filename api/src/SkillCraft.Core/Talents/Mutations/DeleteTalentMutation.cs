using MediatR;
using SkillCraft.Core.Talents.Models;

namespace SkillCraft.Core.Talents.Mutations
{
  public class DeleteTalentMutation : IRequest<TalentModel>
  {
    public DeleteTalentMutation(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
