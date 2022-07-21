using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Natures;
using SkillCraft.Core.Natures.Models;
using SkillCraft.Core.Natures.Payload;

namespace SkillCraft.Web.Controllers
{
  /// <summary>
  /// TODO(fpion): Authorization with World
  /// </summary>
  [ApiController]
  [Route("natures")]
  public class NatureController : ControllerBase
  {
    private readonly INatureService _natureService;

    public NatureController(INatureService natureService)
    {
      _natureService = natureService;
    }

    [HttpPost]
    public async Task<ActionResult<NatureModel>> CreateAsync([FromBody] CreateNaturePayload payload, CancellationToken cancellationToken)
    {
      NatureModel nature = await _natureService.CreateAsync(payload, cancellationToken);
      var uri = new Uri($"/natures/{nature.Id}", UriKind.Relative);

      return Created(uri, nature);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<NatureModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _natureService.DeleteAsync(id, cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<NatureModel>> GetAsync(Core.Attribute? attribute, string? search,
      NatureSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return Ok(await _natureService.GetAsync(attribute, search, sort, desc, index, count, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<NatureModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      NatureModel? nature = await _natureService.GetAsync(id, cancellationToken);
      if (nature == null)
      {
        return NotFound();
      }

      return Ok(nature);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<NatureModel>> UpdateAsync(Guid id, [FromBody] UpdateNaturePayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _natureService.UpdateAsync(id, payload, cancellationToken));
    }
  }
}
