using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Conditions;
using SkillCraft.Core.Conditions.Models;
using SkillCraft.Core.Conditions.Mutations;
using SkillCraft.Core.Conditions.Payloads;
using SkillCraft.Core.Conditions.Queries;
using SkillCraft.Web.Filters;
using SkillCraft.Web.Pipeline;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("conditions")]
  public class ConditionController : ControllerBase
  {
    private readonly IRequestPipeline _pipeline;

    public ConditionController(IRequestPipeline pipeline)
    {
      _pipeline = pipeline;
    }

    [HttpPost]
    public async Task<ActionResult<ConditionModel>> CreateAsync(
      [FromBody] CreateConditionPayload payload,
      CancellationToken cancellationToken
    )
    {
      ConditionModel model = await _pipeline.ExecuteAsync(new CreateConditionMutation(payload), cancellationToken);
      var uri = new Uri($"/conditions/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ConditionModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _pipeline.ExecuteAsync(new DeleteConditionMutation(id), cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<ConditionModel>> GetAsync(
      bool? deleted,
      string? search,
      ConditionSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new GetConditionsQuery
      {
        Deleted = deleted,
        Search = search,
        Sort = sort,
        Desc = desc,
        Index = index,
        Count = count
      }, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ConditionModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      ConditionModel? model = await _pipeline.ExecuteAsync(new GetConditionQuery(id), cancellationToken);
      if (model == null)
      {
        return NotFound();
      }

      return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ConditionModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateConditionPayload payload,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new UpdateConditionMutation(id, payload), cancellationToken));
    }
  }
}
