using AutoMapper;
using Logitar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Aspects;
using SkillCraft.Core.Aspects.Models;
using SkillCraft.Core.Aspects.Payloads;
using SkillCraft.Core.Models;
using SkillCraft.Infrastructure;
using SkillCraft.Web.Filters;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("aspects")]
  public class AspectController : ControllerBase
  {
    private readonly SkillCraftDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IUserContext userContext;

    public AspectController(SkillCraftDbContext dbContext, IMapper mapper, IUserContext userContext)
    {
      this.dbContext = dbContext;
      this.mapper = mapper;
      this.userContext = userContext;
    }

    [HttpPost]
    public async Task<ActionResult<AspectModel>> CreateAsync(
      [FromBody] CreateAspectPayload payload,
      CancellationToken cancellationToken
    )
    {
      var aspect = new Aspect(userContext.Id, userContext.World);
      dbContext.Aspects.Add(aspect);

      AspectModel model = await SaveAsync(aspect, payload, cancellationToken);
      var uri = new Uri($"/aspects/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<AspectModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Aspect? aspect = await dbContext.Aspects.SingleOrDefaultAsync(x => x.Key == id, cancellationToken);
      if (aspect == null)
      {
        return NotFound();
      }
      else if (aspect.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      dbContext.Aspects.Remove(aspect);
      await dbContext.SaveChangesAsync(cancellationToken);

      return Ok(mapper.Map<AspectModel>(aspect));
    }

    [HttpGet]
    public async Task<ActionResult<ListModel<AspectModel>>> GetAsync(
      bool? deleted,
      string? search,
      AspectSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      IQueryable<Aspect> query = dbContext.Aspects
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
          case AspectSort.Name:
            query = desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
            break;
          case AspectSort.UpdatedAt:
            query = desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt);
            break;
          default:
            return BadRequest(new { field = nameof(sort) });
        }
      }

      query = query.ApplyPaging(index, count);

      Aspect[] aspects = await query.ToArrayAsync(cancellationToken);

      return Ok(new ListModel<AspectModel>(
        mapper.Map<IEnumerable<AspectModel>>(aspects),
        total
      ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AspectModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Aspect? aspect = await dbContext.Aspects
        .AsNoTracking()
        .SingleOrDefaultAsync(x => x.Key == id, cancellationToken);

      if (aspect == null)
      {
        return NotFound();
      }
      else if (aspect.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      return Ok(mapper.Map<AspectModel>(aspect));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AspectModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateAspectPayload payload,
      CancellationToken cancellationToken
    )
    {
      Aspect? aspect = await dbContext.Aspects.SingleOrDefaultAsync(x => x.Key == id, cancellationToken);
      if (aspect == null)
      {
        return NotFound();
      }
      else if (aspect.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      aspect.Update(userContext.Id);

      return Ok(await SaveAsync(aspect, payload, cancellationToken));
    }

    private async Task<AspectModel> SaveAsync(Aspect aspect, SaveAspectPayload payload, CancellationToken cancellationToken)
    {
      aspect.Description = payload.Description?.CleanTrim();
      aspect.Name = payload.Name.Trim();

      aspect.MandatoryAttribute1 = payload.MandatoryAttribute1;
      aspect.MandatoryAttribute2 = payload.MandatoryAttribute2;
      aspect.OptionalAttribute1 = payload.OptionalAttribute1;
      aspect.OptionalAttribute2 = payload.OptionalAttribute2;

      aspect.Skill1 = payload.Skill1;
      aspect.Skill2 = payload.Skill2;

      await dbContext.SaveChangesAsync(cancellationToken);

      return mapper.Map<AspectModel>(aspect);
    }
  }
}
