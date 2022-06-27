using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Powers;
using SkillCraft.Core.Powers.Models;
using SkillCraft.Core.Powers.Mutations;
using SkillCraft.Core.Powers.Payloads;
using SkillCraft.Core.Powers.Queries;
using SkillCraft.Web.Filters;
using SkillCraft.Web.Pipeline;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("powers")]
  public class PowerController : ControllerBase
  {
    private readonly IRequestPipeline _pipeline;

    public PowerController(IRequestPipeline pipeline)
    {
      _pipeline = pipeline;
    }

    [HttpPost]
    public async Task<ActionResult<PowerModel>> CreateAsync(
      [FromBody] CreatePowerPayload payload,
      CancellationToken cancellationToken
    )
    {
      PowerModel model = await _pipeline.ExecuteAsync(new CreatePowerMutation(payload), cancellationToken);
      var uri = new Uri($"/powers/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<PowerModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _pipeline.ExecuteAsync(new DeletePowerMutation(id), cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<PowerModel>> GetAsync(
      bool? deleted,
      string? search,
      string? tiers, // TODO(fpion): refactor
      PowerSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new GetPowersQuery
      {
        Deleted = deleted,
        Search = search,
        Tiers = tiers?.Split(',').Select(tier => int.Parse(tier)), // TODO(fpion): refactor
        Sort = sort,
        Desc = desc,
        Index = index,
        Count = count
      }, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PowerModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      PowerModel? model = await _pipeline.ExecuteAsync(new GetPowerQuery(id), cancellationToken);
      if (model == null)
      {
        return NotFound();
      }

      return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PowerModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdatePowerPayload payload,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new UpdatePowerMutation(id, payload), cancellationToken));
    }
  }
}
