using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Educations.Models;

namespace SkillCraft.Core.Educations.Mutations
{
  internal class UpdateEducationMutationHandler : SaveEducationHandler, IRequestHandler<UpdateEducationMutation, EducationModel>
  {
    public UpdateEducationMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<EducationModel> Handle(UpdateEducationMutation request, CancellationToken cancellationToken)
    {
      Education education = await DbContext.Educations
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Education>(request.Id);

      if (education.WorldId != AppContext.World.Id)
      {
        throw new UnauthorizedOperationException<Education>(education, AppContext.UserId, AppContext.World);
      }

      education.Update(AppContext.UserId);

      return await ExecuteAsync(education, request.Payload, cancellationToken);
    }
  }
}
