using AutoMapper;
using Logitar;
using SkillCraft.Core.Educations.Models;
using SkillCraft.Core.Educations.Payloads;

namespace SkillCraft.Core.Educations.Mutations
{
  internal abstract class SaveEducationHandler
  {
    protected SaveEducationHandler(IApplicationContext appContext, IDbContext dbContext, IMapper mapper)
    {
      AppContext = appContext;
      DbContext = dbContext;
      Mapper = mapper;
    }

    protected IApplicationContext AppContext { get; }
    protected IDbContext DbContext { get; }
    protected IMapper Mapper { get; }

    protected async Task<EducationModel> ExecuteAsync(Education education, SaveEducationPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(education);
      ArgumentNullException.ThrowIfNull(payload);

      education.Description = payload.Description?.CleanTrim();
      education.Name = payload.Name.Trim();

      education.Skill = payload.Skill;
      education.WealthMultiplier = payload.WealthMultiplier;

      await DbContext.SaveChangesAsync(cancellationToken);

      AppContext.SetEntity(education);

      return Mapper.Map<EducationModel>(education);
    }
  }
}
