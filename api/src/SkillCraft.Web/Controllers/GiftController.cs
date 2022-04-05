using AutoMapper;
using Logitar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Gifts;
using SkillCraft.Core.Gifts.Models;
using SkillCraft.Core.Gifts.Payloads;
using SkillCraft.Core.Models;
using SkillCraft.Infrastructure;
using SkillCraft.Web.Filters;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("gifts")]
  public class GiftController : ControllerBase
  {
    private readonly SkillCraftDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IUserContext userContext;

    public GiftController(SkillCraftDbContext dbContext, IMapper mapper, IUserContext userContext)
    {
      this.dbContext = dbContext;
      this.mapper = mapper;
      this.userContext = userContext;
    }

    [HttpPost]
    public async Task<ActionResult<GiftModel>> CreateAsync(
      [FromBody] CreateGiftPayload payload,
      CancellationToken cancellationToken
    )
    {
      var gift = new Gift(userContext.Id, userContext.World);
      dbContext.Gifts.Add(gift);

      GiftModel model = await SaveAsync(gift, payload, cancellationToken);
      var uri = new Uri($"/gifts/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<GiftModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Gift? gift = await dbContext.Gifts.SingleOrDefaultAsync(x => x.Key == id, cancellationToken);
      if (gift == null)
      {
        return NotFound();
      }
      else if (gift.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      dbContext.Gifts.Remove(gift);
      await dbContext.SaveChangesAsync(cancellationToken);

      return Ok(mapper.Map<GiftModel>(gift));
    }

    [HttpGet]
    public async Task<ActionResult<ListModel<GiftModel>>> GetAsync(
      bool? deleted,
      string? search,
      GiftSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      IQueryable<Gift> query = dbContext.Gifts
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
          case GiftSort.Name:
            query = desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
            break;
          case GiftSort.UpdatedAt:
            query = desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt);
            break;
          default:
            return BadRequest(new { field = nameof(sort) });
        }
      }

      query = query.ApplyPaging(index, count);

      Gift[] gifts = await query.ToArrayAsync(cancellationToken);

      return Ok(new ListModel<GiftModel>(
        mapper.Map<IEnumerable<GiftModel>>(gifts),
        total
      ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GiftModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Gift? gift = await dbContext.Gifts
        .AsNoTracking()
        .SingleOrDefaultAsync(x => x.Key == id, cancellationToken);

      if (gift == null)
      {
        return NotFound();
      }
      else if (gift.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      return Ok(mapper.Map<GiftModel>(gift));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GiftModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateGiftPayload payload,
      CancellationToken cancellationToken
    )
    {
      Gift? gift = await dbContext.Gifts.SingleOrDefaultAsync(x => x.Key == id, cancellationToken);
      if (gift == null)
      {
        return NotFound();
      }
      else if (gift.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      gift.Update(userContext.Id);

      return Ok(await SaveAsync(gift, payload, cancellationToken));
    }

    private async Task<GiftModel> SaveAsync(Gift gift, SaveGiftPayload payload, CancellationToken cancellationToken)
    {
      gift.Description = payload.Description?.CleanTrim();
      gift.Name = payload.Name.Trim();

      await dbContext.SaveChangesAsync(cancellationToken);

      return mapper.Map<GiftModel>(gift);
    }
  }
}
