using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Talents;
using SkillCraft.Core.Talents.Models;
using SkillCraft.Core.Talents.Mutations;
using SkillCraft.Core.Talents.Payloads;
using SkillCraft.Core.Talents.Queries;
using SkillCraft.Web.Filters;
using SkillCraft.Web.Pipeline;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("talents")]
  public class TalentController : ControllerBase
  {
    private readonly IRequestPipeline _pipeline;

    public TalentController(IRequestPipeline pipeline)
    {
      _pipeline = pipeline;
    }

    [HttpPost]
    public async Task<ActionResult<TalentModel>> CreateAsync(
      [FromBody] CreateTalentPayload payload,
      CancellationToken cancellationToken
    )
    {
      TalentModel model = await _pipeline.ExecuteAsync(new CreateTalentMutation(payload), cancellationToken);
      var uri = new Uri($"/talents/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<TalentModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _pipeline.ExecuteAsync(new DeleteTalentMutation(id), cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<TalentModel>> GetAsync(
      bool? deleted,
      bool? multipleAcquisition,
      string? search,
      string? tiers, // TODO(fpion): refactor
      TalentSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new GetTalentsQuery
      {
        Deleted = deleted,
        MultipleAcquisition = multipleAcquisition,
        Search = search,
        Tiers = tiers?.Split(',').Select(tier => int.Parse(tier)), // TODO(fpion): refactor
        Sort = sort,
        Desc = desc,
        Index = index,
        Count = count
      }, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TalentModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      TalentModel? model = await _pipeline.ExecuteAsync(new GetTalentQuery(id), cancellationToken);
      if (model == null)
      {
        return NotFound();
      }

      return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TalentModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateTalentPayload payload,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new UpdateTalentMutation(id, payload), cancellationToken));
    }
  }
}
