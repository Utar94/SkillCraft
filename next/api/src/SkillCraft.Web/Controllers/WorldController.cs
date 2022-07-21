using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Worlds;
using SkillCraft.Core.Worlds.Models;
using SkillCraft.Core.Worlds.Payload;

namespace SkillCraft.Web.Controllers
{
  /// <summary>
  /// TODO(fpion): Authorization
  /// </summary>
  [ApiController]
  [Route("worlds")]
  public class WorldController : ControllerBase
  {
    private readonly IWorldService _worldService;

    public WorldController(IWorldService worldService)
    {
      _worldService = worldService;
    }

    [HttpPost]
    public async Task<ActionResult<WorldModel>> CreateAsync([FromBody] CreateWorldPayload payload, CancellationToken cancellationToken)
    {
      WorldModel world = await _worldService.CreateAsync(payload, cancellationToken);
      var uri = new Uri($"/worlds/{world.Id}", UriKind.Relative);

      return Created(uri, world);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<WorldModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _worldService.DeleteAsync(id, cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<WorldModel>> GetAsync(string? search,
      WorldSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return Ok(await _worldService.GetAsync(search, sort, desc, index, count, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorldModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      WorldModel? world = await _worldService.GetAsync(id, cancellationToken);
      if (world == null)
      {
        return NotFound();
      }

      return Ok(world);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WorldModel>> UpdateAsync(Guid id, [FromBody] UpdateWorldPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _worldService.UpdateAsync(id, payload, cancellationToken));
    }
  }
}
