using AutoMapper;
using Logitar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Languages;
using SkillCraft.Core.Languages.Models;
using SkillCraft.Core.Languages.Payloads;
using SkillCraft.Core.Models;
using SkillCraft.Infrastructure;
using SkillCraft.Web.Filters;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("languages")]
  public class LanguageController : ControllerBase
  {
    private readonly SkillCraftDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IUserContext userContext;

    public LanguageController(SkillCraftDbContext dbContext, IMapper mapper, IUserContext userContext)
    {
      this.dbContext = dbContext;
      this.mapper = mapper;
      this.userContext = userContext;
    }

    [HttpPost]
    public async Task<ActionResult<LanguageModel>> CreateAsync(
      [FromBody] CreateLanguagePayload payload,
      CancellationToken cancellationToken
    )
    {
      var language = new Language(userContext.Id, userContext.World);
      dbContext.Languages.Add(language);

      LanguageModel model = await SaveAsync(language, payload, cancellationToken);
      var uri = new Uri($"/languages/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpGet]
    public async Task<ActionResult<ListModel<LanguageModel>>> GetAsync(
      bool? deleted,
      string? search,
      LanguageSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      IQueryable<Language> query = dbContext.Languages
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
          case LanguageSort.Name:
            query = desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
            break;
          case LanguageSort.UpdatedAt:
            query = desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt);
            break;
          default:
            return BadRequest(new { field = nameof(sort) });
        }
      }

      query = query.ApplyPaging(index, count);

      Language[] languages = await query.ToArrayAsync(cancellationToken);

      return Ok(new ListModel<LanguageModel>(
        mapper.Map<IEnumerable<LanguageModel>>(languages),
        total
      ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LanguageModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Language? language = await dbContext.Languages
        .AsNoTracking()
        .SingleOrDefaultAsync(x => x.Key == id, cancellationToken);

      if (language == null)
      {
        return NotFound();
      }
      else if (language.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      return Ok(mapper.Map<LanguageModel>(language));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LanguageModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateLanguagePayload payload,
      CancellationToken cancellationToken
    )
    {
      Language? language = await dbContext.Languages.SingleOrDefaultAsync(x => x.Key == id, cancellationToken);
      if (language == null)
      {
        return NotFound();
      }
      else if (language.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      language.Update(userContext.Id);

      return Ok(await SaveAsync(language, payload, cancellationToken));
    }

    [HttpPatch("{id}/delete")]
    public async Task<ActionResult<LanguageModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Language? language = await dbContext.Languages.SingleOrDefaultAsync(x => x.Key == id, cancellationToken);
      if (language == null)
      {
        return NotFound();
      }
      else if (language.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      language.Delete(userContext.Id);

      await dbContext.SaveChangesAsync(cancellationToken);

      return Ok(mapper.Map<LanguageModel>(language));
    }

    private async Task<LanguageModel> SaveAsync(Language language, SaveLanguagePayload payload, CancellationToken cancellationToken)
    {
      language.Description = payload.Description?.CleanTrim();
      language.Exotic = payload.Exotic;
      language.Name = payload.Name.Trim();
      language.Script = payload.Script?.CleanTrim();
      language.TypicalSpeakers = payload.TypicalSpeakers?.CleanTrim();

      await dbContext.SaveChangesAsync(cancellationToken);

      return mapper.Map<LanguageModel>(language);
    }
  }
}
