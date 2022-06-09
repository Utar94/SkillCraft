using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Worlds;
using SkillCraft.Core.Worlds.Models;
using SkillCraft.Core.Worlds.Mutations;
using SkillCraft.Core.Worlds.Payloads;
using SkillCraft.Core.Worlds.Queries;
using SkillCraft.Web.Pipeline;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [Route("worlds")]
  public class WorldController : ControllerBase
  {
    private readonly IRequestPipeline _pipeline;

    public WorldController(IRequestPipeline pipeline)
    {
      _pipeline = pipeline;
    }

    [HttpPost]
    public async Task<ActionResult<WorldModel>> CreateAsync(
      [FromBody] CreateWorldPayload payload,
      CancellationToken cancellationToken
    )
    {
      WorldModel model = await _pipeline.ExecuteAsync(new CreateWorldMutation(payload), cancellationToken);
      var uri = new Uri($"/worlds/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<WorldModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _pipeline.ExecuteAsync(new DeleteWorldMutation(id), cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<WorldModel>> GetAsync(
      string? search,
      WorldSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new GetWorldsQuery
      {
        Search = search,
        Sort = sort,
        Desc = desc,
        Index = index,
        Count = count
      }, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorldModel>> GetAsync(string id, CancellationToken cancellationToken)
    {
      WorldModel? model = await _pipeline.ExecuteAsync(new GetWorldQuery(id), cancellationToken);
      if (model == null)
      {
        return NotFound();
      }

      return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WorldModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateWorldPayload payload,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new UpdateWorldMutation(id, payload), cancellationToken));
    }
  }
}
