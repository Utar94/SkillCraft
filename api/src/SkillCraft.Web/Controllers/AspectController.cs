using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Aspects;
using SkillCraft.Core.Aspects.Models;
using SkillCraft.Core.Aspects.Mutations;
using SkillCraft.Core.Aspects.Payloads;
using SkillCraft.Core.Aspects.Queries;
using SkillCraft.Web.Filters;
using SkillCraft.Web.Pipeline;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("aspects")]
  public class AspectController : ControllerBase
  {
    private readonly IRequestPipeline _pipeline;

    public AspectController(IRequestPipeline pipeline)
    {
      _pipeline = pipeline;
    }

    [HttpPost]
    public async Task<ActionResult<AspectModel>> CreateAsync(
      [FromBody] CreateAspectPayload payload,
      CancellationToken cancellationToken
    )
    {
      AspectModel model = await _pipeline.ExecuteAsync(new CreateAspectMutation(payload), cancellationToken);
      var uri = new Uri($"/aspects/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<AspectModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _pipeline.ExecuteAsync(new DeleteAspectMutation(id), cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<AspectModel>> GetAsync(
      bool? deleted,
      string? search,
      AspectSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new GetAspectsQuery
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
    public async Task<ActionResult<AspectModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      AspectModel? model = await _pipeline.ExecuteAsync(new GetAspectQuery(id), cancellationToken);
      if (model == null)
      {
        return NotFound();
      }

      return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AspectModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateAspectPayload payload,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new UpdateAspectMutation(id, payload), cancellationToken));
    }
  }
}
