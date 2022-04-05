using AutoMapper;
using Logitar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Disabilities;
using SkillCraft.Core.Disabilities.Models;
using SkillCraft.Core.Disabilities.Payloads;
using SkillCraft.Core.Models;
using SkillCraft.Infrastructure;
using SkillCraft.Web.Filters;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("disabilities")]
  public class DisabilityController : ControllerBase
  {
    private readonly SkillCraftDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IUserContext userContext;

    public DisabilityController(SkillCraftDbContext dbContext, IMapper mapper, IUserContext userContext)
    {
      this.dbContext = dbContext;
      this.mapper = mapper;
      this.userContext = userContext;
    }

    [HttpPost]
    public async Task<ActionResult<DisabilityModel>> CreateAsync(
      [FromBody] CreateDisabilityPayload payload,
      CancellationToken cancellationToken
    )
    {
      var disability = new Disability(userContext.Id, userContext.World);
      dbContext.Disabilities.Add(disability);

      DisabilityModel model = await SaveAsync(disability, payload, cancellationToken);
      var uri = new Uri($"/disabilities/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DisabilityModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Disability? disability = await dbContext.Disabilities.SingleOrDefaultAsync(x => x.Key == id, cancellationToken);
      if (disability == null)
      {
        return NotFound();
      }
      else if (disability.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      dbContext.Disabilities.Remove(disability);
      await dbContext.SaveChangesAsync(cancellationToken);

      return Ok(mapper.Map<DisabilityModel>(disability));
    }

    [HttpGet]
    public async Task<ActionResult<ListModel<DisabilityModel>>> GetAsync(
      bool? deleted,
      string? search,
      DisabilitySort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      IQueryable<Disability> query = dbContext.Disabilities
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
          case DisabilitySort.Name:
            query = desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
            break;
          case DisabilitySort.UpdatedAt:
            query = desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt);
            break;
          default:
            return BadRequest(new { field = nameof(sort) });
        }
      }

      query = query.ApplyPaging(index, count);

      Disability[] disabilities = await query.ToArrayAsync(cancellationToken);

      return Ok(new ListModel<DisabilityModel>(
        mapper.Map<IEnumerable<DisabilityModel>>(disabilities),
        total
      ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DisabilityModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Disability? disability = await dbContext.Disabilities
        .AsNoTracking()
        .SingleOrDefaultAsync(x => x.Key == id, cancellationToken);

      if (disability == null)
      {
        return NotFound();
      }
      else if (disability.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      return Ok(mapper.Map<DisabilityModel>(disability));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DisabilityModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateDisabilityPayload payload,
      CancellationToken cancellationToken
    )
    {
      Disability? disability = await dbContext.Disabilities.SingleOrDefaultAsync(x => x.Key == id, cancellationToken);
      if (disability == null)
      {
        return NotFound();
      }
      else if (disability.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      disability.Update(userContext.Id);

      return Ok(await SaveAsync(disability, payload, cancellationToken));
    }

    private async Task<DisabilityModel> SaveAsync(Disability disability, SaveDisabilityPayload payload, CancellationToken cancellationToken)
    {
      disability.Description = payload.Description?.CleanTrim();
      disability.Name = payload.Name.Trim();

      await dbContext.SaveChangesAsync(cancellationToken);

      return mapper.Map<DisabilityModel>(disability);
    }
  }
}
