using AutoMapper;
using Logitar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core;
using SkillCraft.Core.Models;
using SkillCraft.Core.Talents;
using SkillCraft.Core.Talents.Models;
using SkillCraft.Core.Talents.Payloads;
using SkillCraft.Infrastructure;
using SkillCraft.Web.Filters;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("talents")]
  public class TalentController : ControllerBase
  {
    private readonly SkillCraftDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IUserContext userContext;

    public TalentController(SkillCraftDbContext dbContext, IMapper mapper, IUserContext userContext)
    {
      this.dbContext = dbContext;
      this.mapper = mapper;
      this.userContext = userContext;
    }

    [HttpPost]
    public async Task<ActionResult<TalentModel>> CreateAsync(
      [FromBody] CreateTalentPayload payload,
      CancellationToken cancellationToken
    )
    {
      var talent = new Talent(userContext.Id, userContext.World);
      dbContext.Talents.Add(talent);

      TalentModel model = await SaveAsync(talent, payload, cancellationToken);
      var uri = new Uri($"/talents/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpGet]
    public async Task<ActionResult<ListModel<TalentModel>>> GetAsync(
      bool? deleted,
      string? search,
      TalentSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      IQueryable<Talent> query = dbContext.Talents
        .AsNoTracking()
        .Include(x => x.RequiredTalent)
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
          case TalentSort.Name:
            query = desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
            break;
          case TalentSort.UpdatedAt:
            query = desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt);
            break;
          default:
            return BadRequest(new { field = nameof(sort) });
        }
      }

      query = query.ApplyPaging(index, count);

      Talent[] talents = await query.ToArrayAsync(cancellationToken);

      return Ok(new ListModel<TalentModel>(
        mapper.Map<IEnumerable<TalentModel>>(talents),
        total
      ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TalentModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Talent? talent = await dbContext.Talents
        .AsNoTracking()
        .Include(x => x.RequiredTalent)
        .SingleOrDefaultAsync(x => x.Key == id, cancellationToken);

      if (talent == null)
      {
        return NotFound();
      }
      else if (talent.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      return Ok(mapper.Map<TalentModel>(talent));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TalentModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateTalentPayload payload,
      CancellationToken cancellationToken
    )
    {
      Talent? talent = await dbContext.Talents
        .Include(x => x.RequiredTalent)
        .SingleOrDefaultAsync(x => x.Key == id, cancellationToken);
      if (talent == null)
      {
        return NotFound();
      }
      else if (talent.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      talent.Update(userContext.Id);

      return Ok(await SaveAsync(talent, payload, cancellationToken));
    }

    [HttpPatch("{id}/delete")]
    public async Task<ActionResult<TalentModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Talent? talent = await dbContext.Talents
        .Include(x => x.RequiredTalent)
        .SingleOrDefaultAsync(x => x.Key == id, cancellationToken);
      if (talent == null)
      {
        return NotFound();
      }
      else if (talent.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      talent.Delete(userContext.Id);

      await dbContext.SaveChangesAsync(cancellationToken);

      return Ok(mapper.Map<TalentModel>(talent));
    }

    private async Task<TalentModel> SaveAsync(Talent talent, SaveTalentPayload payload, CancellationToken cancellationToken)
    {
      Talent? requiredTalent = null;
      if (payload.RequiredTalentId.HasValue)
      {
        requiredTalent = await dbContext.Talents
          .SingleOrDefaultAsync(x => x.Key == payload.RequiredTalentId.Value, cancellationToken)
          ?? throw new EntityNotFoundException<Talent>(payload.RequiredTalentId.Value, nameof(payload.RequiredTalentId));
      }

      talent.Description = payload.Description?.CleanTrim();
      talent.MultipleAcquisitions = payload.MultipleAcquisitions;
      talent.Name = payload.Name.Trim();
      talent.RequiredTalent = requiredTalent;
      talent.RequiredTalentId = requiredTalent?.Id;
      talent.Tier = payload.Tier;

      await dbContext.SaveChangesAsync(cancellationToken);

      return mapper.Map<TalentModel>(talent);
    }
  }
}
