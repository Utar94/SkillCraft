using AutoMapper;
using Logitar;
using Logitar.Identity.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Models;
using SkillCraft.Core.Worlds;
using SkillCraft.Infrastructure;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [Route("worlds")]
  public class WorldController : ControllerBase
  {
    private readonly SkillCraftDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IUserContext userContext;

    public WorldController(SkillCraftDbContext dbContext, IMapper mapper, IUserContext userContext)
    {
      this.dbContext = dbContext;
      this.mapper = mapper;
      this.userContext = userContext;
    }

    [HttpPost]
    public async Task<ActionResult<WorldModel>> CreateAsync(
      [FromBody] CreateWorldPayload payload,
      CancellationToken cancellationToken
    )
    {
      string alias = payload.Alias.ToLowerInvariant();
      World? world = await dbContext.Worlds.SingleOrDefaultAsync(x => x.Alias == alias, cancellationToken);
      if (world != null)
      {
        return Conflict(new { field = nameof(payload.Alias) });
      }

      world = new World(alias, userContext.Id);

      dbContext.Worlds.Add(world);

      WorldModel model = await SaveAsync(world, payload, cancellationToken);
      var uri = new Uri($"/worlds/{world.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<WorldModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      World? world = await dbContext.Worlds.SingleOrDefaultAsync(x => x.Key == id, cancellationToken);

      if (world == null)
      {
        return NotFound();
      }
      else if (world.CreatedById != userContext.Id)
      {
        return Forbid();
      }

      dbContext.Worlds.Remove(world);
      await dbContext.SaveChangesAsync(cancellationToken);

      return Ok(mapper.Map<WorldModel>(world));
    }

    [HttpGet]
    public async Task<ActionResult<ListModel<WorldModel>>> GetAsync(
      string? search,
      WorldSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      IQueryable<World> query = dbContext.Worlds
        .AsNoTracking()
        .Where(x => x.CreatedById == userContext.Id);

      if (search != null)
      {
        query = query.Where(x => x.Alias.Contains(search) || x.Name.Contains(search));
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        switch (sort.Value)
        {
          case WorldSort.Name:
            query = desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
            break;
          case WorldSort.UpdatedAt:
            query = desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt);
            break;
          default:
            return BadRequest(new { field = nameof(sort) });
        }
      }

      query = query.ApplyPaging(index, count);

      World[] worlds = await query.ToArrayAsync(cancellationToken);

      return Ok(new ListModel<WorldModel>(mapper.Map<IEnumerable<WorldModel>>(worlds), total));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorldModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      World? world = await dbContext.Worlds
        .AsNoTracking()
        .SingleOrDefaultAsync(x => x.Key == id, cancellationToken);

      if (world == null)
      {
        return NotFound();
      }
      else if (world.CreatedById != userContext.Id)
      {
        return Forbid();
      }

      return Ok(mapper.Map<WorldModel>(world));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WorldModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateWorldPayload payload,
      CancellationToken cancellationToken
    )
    {
      World? world = await dbContext.Worlds.SingleOrDefaultAsync(x => x.Key == id, cancellationToken);

      if (world == null)
      {
        return NotFound();
      }
      else if (world.CreatedById != userContext.Id)
      {
        return Forbid();
      }

      world.Update(userContext.Id);

      return Ok(await SaveAsync(world, payload, cancellationToken));
    }

    private async Task<WorldModel> SaveAsync(World world, SaveWorldPayload payload, CancellationToken cancellationToken)
    {
      world.Confidentiality = payload.Confidentiality;
      world.Description = payload.Description?.CleanTrim();
      world.Name = payload.Name.Trim();

      await dbContext.SaveChangesAsync(cancellationToken);

      return mapper.Map<WorldModel>(world);
    }
  }
}
