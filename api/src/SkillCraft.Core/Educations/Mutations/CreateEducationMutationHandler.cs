using AutoMapper;
using MediatR;
using SkillCraft.Core.Educations.Models;

namespace SkillCraft.Core.Educations.Mutations
{
  internal class CreateEducationMutationHandler : SaveEducationHandler, IRequestHandler<CreateEducationMutation, EducationModel>
  {
    public CreateEducationMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
      : base(appContext, dbContext, mapper)
    {
    }

    public async Task<EducationModel> Handle(CreateEducationMutation request, CancellationToken cancellationToken)
    {
      var education = new Education(AppContext.UserId, AppContext.World);

      DbContext.Educations.Add(education);

      return await ExecuteAsync(education, request.Payload, cancellationToken);
    }
  }
}
