using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Classes.Models;

namespace SkillCraft.Core.Classes.Mutations
{
  internal class UpdateClassMutationHandler : SaveClassHandler, IRequestHandler<UpdateClassMutation, ClassModel>
  {
    public UpdateClassMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<ClassModel> Handle(UpdateClassMutation request, CancellationToken cancellationToken)
    {
      Class @class = await DbContext.Classes
        .Include(x => x.Talents).ThenInclude(x => x.Talent)
        .Include(x => x.UniqueTalent)
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Class>(request.Id);

      if (@class.WorldId != AppContext.World.Id)
      {
        throw new UnauthorizedOperationException<Class>(@class, AppContext.UserId, AppContext.World);
      }

      @class.Update(AppContext.UserId);

      return await ExecuteAsync(@class, request.Payload, cancellationToken);
    }
  }
}
