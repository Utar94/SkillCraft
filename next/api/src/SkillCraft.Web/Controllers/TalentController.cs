using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core;
using SkillCraft.Core.Talents;
using SkillCraft.Core.Talents.Models;
using SkillCraft.Core.Talents.Payload;

namespace SkillCraft.Web.Controllers
{
  /// <summary>
  /// TODO(fpion): Authorization with World
  /// </summary>
  [ApiController]
  [Route("talents")]
  public class TalentController : ControllerBase
  {
    private readonly ITalentService _talentService;

    public TalentController(ITalentService talentService)
    {
      _talentService = talentService;
    }

    [HttpPost]
    public async Task<ActionResult<TalentModel>> CreateAsync([FromBody] CreateTalentPayload payload, CancellationToken cancellationToken)
    {
      TalentModel talent = await _talentService.CreateAsync(payload, cancellationToken);
      var uri = new Uri($"/talents/{talent.Id}", UriKind.Relative);

      return Created(uri, talent);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<TalentModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _talentService.DeleteAsync(id, cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<TalentModel>> GetAsync(bool? multipleAcquisition, string? search, Skill? skill, [FromQuery] IEnumerable<string> tiers,
      TalentSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      IEnumerable<int>? parsedTiers = tiers.Where(x => int.TryParse(x, out _)).Select(x => int.Parse(x));
      parsedTiers = parsedTiers.Any() ? parsedTiers : null;

      return Ok(await _talentService.GetAsync(multipleAcquisition, search, skill, parsedTiers, sort, desc, index, count, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TalentModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      TalentModel? talent = await _talentService.GetAsync(id, cancellationToken);
      if (talent == null)
      {
        return NotFound();
      }

      return Ok(talent);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TalentModel>> UpdateAsync(Guid id, [FromBody] UpdateTalentPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _talentService.UpdateAsync(id, payload, cancellationToken));
    }
  }
}
