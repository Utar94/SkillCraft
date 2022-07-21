using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core;
using SkillCraft.Core.Castes;
using SkillCraft.Core.Castes.Models;
using SkillCraft.Core.Castes.Payload;

namespace SkillCraft.Web.Controllers
{
  /// <summary>
  /// TODO(fpion): Authorization with World
  /// </summary>
  [ApiController]
  [Route("castes")]
  public class CasteController : ControllerBase
  {
    private readonly ICasteService _casteService;

    public CasteController(ICasteService casteService)
    {
      _casteService = casteService;
    }

    [HttpPost]
    public async Task<ActionResult<CasteModel>> CreateAsync([FromBody] CreateCastePayload payload, CancellationToken cancellationToken)
    {
      CasteModel caste = await _casteService.CreateAsync(payload, cancellationToken);
      var uri = new Uri($"/castes/{caste.Id}", UriKind.Relative);

      return Created(uri, caste);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<CasteModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _casteService.DeleteAsync(id, cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<CasteModel>> GetAsync(string? search, Skill? skill,
      CasteSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return Ok(await _casteService.GetAsync(search, skill, sort, desc, index, count, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CasteModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      CasteModel? caste = await _casteService.GetAsync(id, cancellationToken);
      if (caste == null)
      {
        return NotFound();
      }

      return Ok(caste);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CasteModel>> UpdateAsync(Guid id, [FromBody] UpdateCastePayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _casteService.UpdateAsync(id, payload, cancellationToken));
    }
  }
}
