using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Aspects;
using SkillCraft.Core.Aspects.Models;
using SkillCraft.Core.Aspects.Payload;

namespace SkillCraft.Web.Controllers
{
  /// <summary>
  /// TODO(fpion): Authorization with World
  /// </summary>
  [ApiController]
  [Route("aspects")]
  public class AspectController : ControllerBase
  {
    private readonly IAspectService _aspectService;

    public AspectController(IAspectService aspectService)
    {
      _aspectService = aspectService;
    }

    [HttpPost]
    public async Task<ActionResult<AspectModel>> CreateAsync([FromBody] CreateAspectPayload payload, CancellationToken cancellationToken)
    {
      AspectModel aspect = await _aspectService.CreateAsync(payload, cancellationToken);
      var uri = new Uri($"/aspects/{aspect.Id}", UriKind.Relative);

      return Created(uri, aspect);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<AspectModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _aspectService.DeleteAsync(id, cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<AspectModel>> GetAsync(string? search,
      AspectSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return Ok(await _aspectService.GetAsync(search, sort, desc, index, count, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AspectModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      AspectModel? aspect = await _aspectService.GetAsync(id, cancellationToken);
      if (aspect == null)
      {
        return NotFound();
      }

      return Ok(aspect);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AspectModel>> UpdateAsync(Guid id, [FromBody] UpdateAspectPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _aspectService.UpdateAsync(id, payload, cancellationToken));
    }
  }
}
