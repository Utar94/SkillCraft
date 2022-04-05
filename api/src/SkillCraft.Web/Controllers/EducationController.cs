using AutoMapper;
using Logitar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Educations;
using SkillCraft.Core.Educations.Models;
using SkillCraft.Core.Educations.Payloads;
using SkillCraft.Core.Models;
using SkillCraft.Infrastructure;
using SkillCraft.Web.Filters;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("educations")]
  public class EducationController : ControllerBase
  {
    private readonly SkillCraftDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IUserContext userContext;

    public EducationController(SkillCraftDbContext dbContext, IMapper mapper, IUserContext userContext)
    {
      this.dbContext = dbContext;
      this.mapper = mapper;
      this.userContext = userContext;
    }

    [HttpPost]
    public async Task<ActionResult<EducationModel>> CreateAsync(
      [FromBody] CreateEducationPayload payload,
      CancellationToken cancellationToken
    )
    {
      var education = new Education(userContext.Id, userContext.World);
      dbContext.Educations.Add(education);

      EducationModel model = await SaveAsync(education, payload, cancellationToken);
      var uri = new Uri($"/educations/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpGet]
    public async Task<ActionResult<ListModel<EducationModel>>> GetAsync(
      bool? deleted,
      string? search,
      EducationSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      IQueryable<Education> query = dbContext.Educations
        .AsNoTracking()
        .Where(x => x.WorldId == userContext.World.Id);

      if (deleted.HasValue)
      {
        query = query.Where(x => x.Deleted == deleted.Value);
      }
      if (search != null)
      {
        query = query.Where(x => x.Name.Contains(search));
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        switch (sort.Value)
        {
          case EducationSort.Name:
            query = desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
            break;
          case EducationSort.UpdatedAt:
            query = desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt);
            break;
          default:
            return BadRequest(new { field = nameof(sort) });
        }
      }

      query = query.ApplyPaging(index, count);

      Education[] educations = await query.ToArrayAsync(cancellationToken);

      return Ok(new ListModel<EducationModel>(
        mapper.Map<IEnumerable<EducationModel>>(educations),
        total
      ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EducationModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Education? education = await dbContext.Educations
        .AsNoTracking()
        .SingleOrDefaultAsync(x => x.Key == id, cancellationToken);

      if (education == null)
      {
        return NotFound();
      }
      else if (education.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      return Ok(mapper.Map<EducationModel>(education));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EducationModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateEducationPayload payload,
      CancellationToken cancellationToken
    )
    {
      Education? education = await dbContext.Educations.SingleOrDefaultAsync(x => x.Key == id, cancellationToken);
      if (education == null)
      {
        return NotFound();
      }
      else if (education.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      education.Update(userContext.Id);

      return Ok(await SaveAsync(education, payload, cancellationToken));
    }

    [HttpPatch("{id}/delete")]
    public async Task<ActionResult<EducationModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Education? education = await dbContext.Educations.SingleOrDefaultAsync(x => x.Key == id, cancellationToken);
      if (education == null)
      {
        return NotFound();
      }
      else if (education.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      education.Delete(userContext.Id);

      await dbContext.SaveChangesAsync(cancellationToken);

      return Ok(mapper.Map<EducationModel>(education));
    }

    private async Task<EducationModel> SaveAsync(Education education, SaveEducationPayload payload, CancellationToken cancellationToken)
    {
      education.Description = payload.Description?.CleanTrim();
      education.Name = payload.Name.Trim();
      education.Skill = payload.Skill;
      education.WealthMultiplier = payload.WealthMultiplier;

      await dbContext.SaveChangesAsync(cancellationToken);

      return mapper.Map<EducationModel>(education);
    }
  }
}
