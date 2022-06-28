using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Classes;
using SkillCraft.Core.Classes.Models;
using SkillCraft.Core.Classes.Mutations;
using SkillCraft.Core.Classes.Payloads;
using SkillCraft.Core.Classes.Queries;
using SkillCraft.Web.Filters;
using SkillCraft.Web.Pipeline;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("classes")]
  public class ClassController : ControllerBase
  {
    private readonly IRequestPipeline _pipeline;

    public ClassController(IRequestPipeline pipeline)
    {
      _pipeline = pipeline;
    }

    [HttpPost]
    public async Task<ActionResult<ClassModel>> CreateAsync(
      [FromBody] CreateClassPayload payload,
      CancellationToken cancellationToken
    )
    {
      ClassModel model = await _pipeline.ExecuteAsync(new CreateClassMutation(payload), cancellationToken);
      var uri = new Uri($"/classes/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ClassModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _pipeline.ExecuteAsync(new DeleteClassMutation(id), cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<ClassModel>> GetAsync(
      bool? deleted,
      string? search,
      string? tiers, // TODO(fpion): refactor
      ClassSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new GetClassesQuery
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
    public async Task<ActionResult<ClassModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      ClassModel? model = await _pipeline.ExecuteAsync(new GetClassQuery(id), cancellationToken);
      if (model == null)
      {
        return NotFound();
      }

      return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ClassModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateClassPayload payload,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new UpdateClassMutation(id, payload), cancellationToken));
    }
  }
}
