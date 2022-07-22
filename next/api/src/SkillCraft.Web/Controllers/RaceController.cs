using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core;
using SkillCraft.Core.Races;
using SkillCraft.Core.Races.Models;
using SkillCraft.Core.Races.Payload;

namespace SkillCraft.Web.Controllers
{
  /// <summary>
  /// TODO(fpion): Authorization with World
  /// </summary>
  [ApiController]
  [Route("races")]
  public class RaceController : ControllerBase
  {
    private readonly IRaceService _raceService;

    public RaceController(IRaceService raceService)
    {
      _raceService = raceService;
    }

    [HttpPost]
    public async Task<ActionResult<RaceModel>> CreateAsync([FromBody] CreateRacePayload payload, CancellationToken cancellationToken)
    {
      RaceModel race = await _raceService.CreateAsync(payload, cancellationToken);
      var uri = new Uri($"/races/{race.Id}", UriKind.Relative);

      return Created(uri, race);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<RaceModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _raceService.DeleteAsync(id, cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<RaceModel>> GetAsync(Guid? parentId, string? search, SizeCategory? size,
      RaceSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return Ok(await _raceService.GetAsync(parentId, search, size, sort, desc, index, count, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RaceModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      RaceModel? race = await _raceService.GetAsync(id, cancellationToken);
      if (race == null)
      {
        return NotFound();
      }

      return Ok(race);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RaceModel>> UpdateAsync(Guid id, [FromBody] UpdateRacePayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _raceService.UpdateAsync(id, payload, cancellationToken));
    }
  }
}
