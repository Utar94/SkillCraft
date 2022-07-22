using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Powers;
using SkillCraft.Core.Powers.Models;
using SkillCraft.Core.Powers.Payload;

namespace SkillCraft.Web.Controllers
{
  /// <summary>
  /// TODO(fpion): Authorization with World
  /// </summary>
  [ApiController]
  [Route("powers")]
  public class PowerController : ControllerBase
  {
    private readonly IPowerService _powerService;

    public PowerController(IPowerService powerService)
    {
      _powerService = powerService;
    }

    [HttpPost]
    public async Task<ActionResult<PowerModel>> CreateAsync([FromBody] CreatePowerPayload payload, CancellationToken cancellationToken)
    {
      PowerModel power = await _powerService.CreateAsync(payload, cancellationToken);
      var uri = new Uri($"/powers/{power.Id}", UriKind.Relative);

      return Created(uri, power);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<PowerModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _powerService.DeleteAsync(id, cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<PowerModel>> GetAsync(string? search, [FromQuery] IEnumerable<string> tiers,
      PowerSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      IEnumerable<int>? parsedTiers = tiers.Where(x => int.TryParse(x, out _)).Select(x => int.Parse(x));
      parsedTiers = parsedTiers.Any() ? parsedTiers : null;

      return Ok(await _powerService.GetAsync(search, parsedTiers, sort, desc, index, count, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PowerModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      PowerModel? power = await _powerService.GetAsync(id, cancellationToken);
      if (power == null)
      {
        return NotFound();
      }

      return Ok(power);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PowerModel>> UpdateAsync(Guid id, [FromBody] UpdatePowerPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _powerService.UpdateAsync(id, payload, cancellationToken));
    }
  }
}
