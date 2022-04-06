using AutoMapper;
using Logitar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Core.Characters;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Characters.Payloads;
using SkillCraft.Core.Models;
using SkillCraft.Infrastructure;
using SkillCraft.Web.Filters;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("characters")]
  public class CharacterController : ControllerBase
  {
    private readonly SkillCraftDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IUserContext userContext;

    public CharacterController(SkillCraftDbContext dbContext, IMapper mapper, IUserContext userContext)
    {
      this.dbContext = dbContext;
      this.mapper = mapper;
      this.userContext = userContext;
    }

    [HttpPost]
    public async Task<ActionResult<CharacterModel>> CreateAsync(
      [FromBody] CreateCharacterPayload payload,
      CancellationToken cancellationToken
    )
    {
      var character = new Character(userContext.Id, userContext.World);
      dbContext.Characters.Add(character);

      CharacterModel model = await SaveAsync(character, payload, cancellationToken);
      var uri = new Uri($"/characters/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpGet]
    public async Task<ActionResult<ListModel<CharacterModel>>> GetAsync(
      bool? deleted,
      string? search,
      CharacterSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      IQueryable<Character> query = dbContext.Characters
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
          case CharacterSort.Name:
            query = desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
            break;
          case CharacterSort.UpdatedAt:
            query = desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt);
            break;
          default:
            return BadRequest(new { field = nameof(sort) });
        }
      }

      query = query.ApplyPaging(index, count);

      Character[] characters = await query.ToArrayAsync(cancellationToken);

      return Ok(new ListModel<CharacterModel>(
        mapper.Map<IEnumerable<CharacterModel>>(characters),
        total
      ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CharacterModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Character? character = await dbContext.Characters
        .AsNoTracking()
        .SingleOrDefaultAsync(x => x.Key == id, cancellationToken);

      if (character == null)
      {
        return NotFound();
      }
      else if (character.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      return Ok(mapper.Map<CharacterModel>(character));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CharacterModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateCharacterPayload payload,
      CancellationToken cancellationToken
    )
    {
      Character? character = await dbContext.Characters.SingleOrDefaultAsync(x => x.Key == id, cancellationToken);
      if (character == null)
      {
        return NotFound();
      }
      else if (character.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      character.Update(userContext.Id);

      return Ok(await SaveAsync(character, payload, cancellationToken));
    }

    [HttpPatch("{id}/delete")]
    public async Task<ActionResult<CharacterModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Character? character = await dbContext.Characters.SingleOrDefaultAsync(x => x.Key == id, cancellationToken);
      if (character == null)
      {
        return NotFound();
      }
      else if (character.WorldId != userContext.World.Id)
      {
        return Forbid();
      }

      character.Delete(userContext.Id);

      await dbContext.SaveChangesAsync(cancellationToken);

      return Ok(mapper.Map<CharacterModel>(character));
    }

    private async Task<CharacterModel> SaveAsync(Character character, SaveCharacterPayload payload, CancellationToken cancellationToken)
    {
      character.Description = payload.Description?.CleanTrim();
      character.Name = payload.Name.Trim();
      character.Player = payload.Player?.CleanTrim();

      character.Stature = payload.Stature;
      character.Weight = payload.Weight;
      character.Age = payload.Age;

      character.Experience = payload.Experience;
      character.Vitality = payload.Vitality;
      character.Stamina = payload.Stamina;

      character.BloodAlcoholContent = payload.BloodAlcoholContent;
      character.Intoxication = payload.Intoxication;

      await dbContext.SaveChangesAsync(cancellationToken);

      return mapper.Map<CharacterModel>(character);
    }
  }
}
