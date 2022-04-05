using AutoMapper;
using Logitar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Natures;
using SkillCraft.Core.Natures.Models;
using SkillCraft.Core.Natures.Payloads;
using SkillCraft.Core.Models;
using SkillCraft.Infrastructure;
using SkillCraft.Core.Gifts;
using SkillCraft.Core;
using SkillCraft.Web.Filters;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("natures")]
  public class NatureController : ControllerBase
  {
    private readonly SkillCraftDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IUserContext userContext;

    public NatureController(SkillCraftDbContext dbContext, IMapper mapper, IUserContext userContext)
    {
      this.dbContext = dbContext;
      this.mapper = mapper;
      this.userContext = userContext;
    }

    [HttpPost]
    public async Task<ActionResult<NatureModel>> CreateAsync(
      [FromBody] CreateNaturePayload payload,
      CancellationToken cancellationToken
    )
    {
      var nature = new Nature(userContext.Id, userContext.World);
      dbContext.Natures.Add(nature);

      NatureModel model = await SaveAsync(nature, payload, cancellationToken);
      var uri = new Uri($"/natures/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<NatureModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Nature? nature = await dbContext.Natures
        .Include(x => x.Gift)
        .SingleOrDefaultAsync(x => x.Key == id, cancellationToken);
      if (nature == null)
      {
        return NotFound();
      }
      else if (nature.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      dbContext.Natures.Remove(nature);
      await dbContext.SaveChangesAsync(cancellationToken);

      return Ok(mapper.Map<NatureModel>(nature));
    }

    [HttpGet]
    public async Task<ActionResult<ListModel<NatureModel>>> GetAsync(
      bool? deleted,
      string? search,
      NatureSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      IQueryable<Nature> query = dbContext.Natures
        .AsNoTracking()
        .Include(x => x.Gift)
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
          case NatureSort.Name:
            query = desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
            break;
          case NatureSort.UpdatedAt:
            query = desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt);
            break;
          default:
            return BadRequest(new { field = nameof(sort) });
        }
      }

      query = query.ApplyPaging(index, count);

      Nature[] natures = await query.ToArrayAsync(cancellationToken);

      return Ok(new ListModel<NatureModel>(
        mapper.Map<IEnumerable<NatureModel>>(natures),
        total
      ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<NatureModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Nature? nature = await dbContext.Natures
        .AsNoTracking()
        .Include(x => x.Gift)
        .SingleOrDefaultAsync(x => x.Key == id, cancellationToken);

      if (nature == null)
      {
        return NotFound();
      }
      else if (nature.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      return Ok(mapper.Map<NatureModel>(nature));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<NatureModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateNaturePayload payload,
      CancellationToken cancellationToken
    )
    {
      Nature? nature = await dbContext.Natures
        .Include(x => x.Gift)
        .SingleOrDefaultAsync(x => x.Key == id, cancellationToken);
      if (nature == null)
      {
        return NotFound();
      }
      else if (nature.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      nature.Update(userContext.Id);

      return Ok(await SaveAsync(nature, payload, cancellationToken));
    }

    private async Task<NatureModel> SaveAsync(Nature nature, SaveNaturePayload payload, CancellationToken cancellationToken)
    {
      Gift? gift = null;
      if (payload.GiftId.HasValue)
      {
        gift = await dbContext.Gifts
          .SingleOrDefaultAsync(x => x.Key == payload.GiftId.Value, cancellationToken)
          ?? throw new EntityNotFoundException<Gift>(payload.GiftId.Value, nameof(payload.GiftId));
      }

      nature.Attribute = payload.Attribute;
      nature.Description = payload.Description?.CleanTrim();
      nature.Gift = gift;
      nature.GiftId = gift?.Id;
      nature.Name = payload.Name.Trim();

      await dbContext.SaveChangesAsync(cancellationToken);

      return mapper.Map<NatureModel>(nature);
    }
  }
}
