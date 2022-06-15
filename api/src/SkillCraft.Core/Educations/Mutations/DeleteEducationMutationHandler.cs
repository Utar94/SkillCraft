using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Educations.Models;

namespace SkillCraft.Core.Educations.Mutations
{
  internal class DeleteEducationMutationHandler : IRequestHandler<DeleteEducationMutation, EducationModel>
  {
    private readonly IApplicationContext _appContext;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public DeleteEducationMutationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      _appContext = appContext;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<EducationModel> Handle(DeleteEducationMutation request, CancellationToken cancellationToken)
    {
      Education education = await _dbContext.Educations
        .SingleOrDefaultAsync(x => x.Uuid == request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Education>(request.Id);

      if (education.WorldId != _appContext.World.Id)
      {
        throw new UnauthorizedOperationException<Education>(education, _appContext.UserId, _appContext.World);
      }

      education.Delete(_appContext.UserId);
      await _dbContext.SaveChangesAsync(cancellationToken);

      return _mapper.Map<EducationModel>(education);
    }
  }
}
