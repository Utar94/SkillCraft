using AutoMapper;
using Logitar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Castes;
using SkillCraft.Core.Castes.Models;
using SkillCraft.Core.Castes.Payloads;
using SkillCraft.Core.Models;
using SkillCraft.Infrastructure;
using SkillCraft.Web.Filters;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("castes")]
  public class CasteController : ControllerBase
  {
    private readonly SkillCraftDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IUserContext userContext;

    public CasteController(SkillCraftDbContext dbContext, IMapper mapper, IUserContext userContext)
    {
      this.dbContext = dbContext;
      this.mapper = mapper;
      this.userContext = userContext;
    }

    [HttpPost]
    public async Task<ActionResult<CasteModel>> CreateAsync(
      [FromBody] CreateCastePayload payload,
      CancellationToken cancellationToken
    )
    {
      var caste = new Caste(userContext.Id, userContext.World);
      dbContext.Castes.Add(caste);

      CasteModel model = await SaveAsync(caste, payload, cancellationToken);
      var uri = new Uri($"/castes/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpGet]
    public async Task<ActionResult<ListModel<CasteModel>>> GetAsync(
      bool? deleted,
      string? search,
      CasteSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      IQueryable<Caste> query = dbContext.Castes
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
          case CasteSort.Name:
            query = desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
            break;
          case CasteSort.UpdatedAt:
            query = desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt);
            break;
          default:
            return BadRequest(new { field = nameof(sort) });
        }
      }

      query = query.ApplyPaging(index, count);

      Caste[] castes = await query.ToArrayAsync(cancellationToken);

      return Ok(new ListModel<CasteModel>(
        mapper.Map<IEnumerable<CasteModel>>(castes),
        total
      ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CasteModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Caste? caste = await dbContext.Castes
        .AsNoTracking()
        .Include(x => x.Traits)
        .SingleOrDefaultAsync(x => x.Key == id, cancellationToken);

      if (caste == null)
      {
        return NotFound();
      }
      else if (caste.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      return Ok(mapper.Map<CasteModel>(caste));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CasteModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateCastePayload payload,
      CancellationToken cancellationToken
    )
    {
      Caste? caste = await dbContext.Castes
        .Include(x => x.Traits)
        .SingleOrDefaultAsync(x => x.Key == id, cancellationToken);
      if (caste == null)
      {
        return NotFound();
      }
      else if (caste.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      caste.Update(userContext.Id);

      return Ok(await SaveAsync(caste, payload, cancellationToken));
    }

    [HttpPatch("{id}/delete")]
    public async Task<ActionResult<CasteModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Caste? caste = await dbContext.Castes
        .Include(x => x.Traits)
        .SingleOrDefaultAsync(x => x.Key == id, cancellationToken);
      if (caste == null)
      {
        return NotFound();
      }
      else if (caste.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      caste.Delete(userContext.Id);

      await dbContext.SaveChangesAsync(cancellationToken);

      return Ok(mapper.Map<CasteModel>(caste));
    }

    private async Task<CasteModel> SaveAsync(Caste caste, SaveCastePayload payload, CancellationToken cancellationToken)
    {
      caste.Description = payload.Description?.CleanTrim();
      caste.Name = payload.Name.Trim();
      caste.Skill = payload.Skill;
      caste.WealthRoll = payload.WealthRoll?.CleanTrim();

      Dictionary<Guid, CasteTrait> traits = caste.Traits.ToDictionary(x => x.Key, x => x);

      var traitPayloads = payload.Traits ?? Enumerable.Empty<CasteTraitPayload>();
      var missingTraits = new List<Guid>(capacity: traitPayloads.Count());
      foreach (CasteTraitPayload traitPayload in traitPayloads)
      {
        if (traitPayload.Id.HasValue)
        {
          if (traits.TryGetValue(traitPayload.Id.Value, out CasteTrait? trait))
          {
            trait.Description = traitPayload.Description?.CleanTrim();
            trait.Name = traitPayload.Name.Trim();
          }
          else
          {
            missingTraits.Add(traitPayload.Id.Value);
          }
        }
        else
        {
          caste.Traits.Add(new CasteTrait(caste)
          {
            Description = traitPayload.Description?.CleanTrim(),
            Name = traitPayload.Name.Trim()
          });
        }
      }

      HashSet<Guid> traitIds = traitPayloads.Where(x => x.Id.HasValue).Select(x => x.Id!.Value).ToHashSet();
      foreach (CasteTrait trait in traits.Values)
      {
        if (!traitIds.Contains(trait.Key))
        {
          caste.Traits.Remove(trait);
        }
      }

      if (missingTraits.Any())
      {
        throw new CasteTraitsNotFoundException(missingTraits);
      }

      await dbContext.SaveChangesAsync(cancellationToken);

      return mapper.Map<CasteModel>(caste);
    }
  }
}
