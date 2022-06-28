using AutoMapper;
using MediatR;
using SkillCraft.Core.Classes.Models;

namespace SkillCraft.Core.Classes.Mutations
{
  internal class CreateClassMutationHandler : SaveClassHandler, IRequestHandler<CreateClassMutation, ClassModel>
  {
    public CreateClassMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<ClassModel> Handle(CreateClassMutation request, CancellationToken cancellationToken)
    {
      var @class = new Class(request.Payload.Tier, AppContext.UserId, AppContext.World);

      DbContext.Classes.Add(@class);

      return await ExecuteAsync(@class, request.Payload, cancellationToken);
    }
  }
}
