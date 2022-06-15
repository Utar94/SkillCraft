using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Educations;
using SkillCraft.Core.Educations.Models;
using SkillCraft.Core.Educations.Mutations;
using SkillCraft.Core.Educations.Payloads;
using SkillCraft.Core.Educations.Queries;
using SkillCraft.Web.Filters;
using SkillCraft.Web.Pipeline;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("educations")]
  public class EducationController : ControllerBase
  {
    private readonly IRequestPipeline _pipeline;

    public EducationController(IRequestPipeline pipeline)
    {
      _pipeline = pipeline;
    }

    [HttpPost]
    public async Task<ActionResult<EducationModel>> CreateAsync(
      [FromBody] CreateEducationPayload payload,
      CancellationToken cancellationToken
    )
    {
      EducationModel model = await _pipeline.ExecuteAsync(new CreateEducationMutation(payload), cancellationToken);
      var uri = new Uri($"/educations/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<EducationModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _pipeline.ExecuteAsync(new DeleteEducationMutation(id), cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<EducationModel>> GetAsync(
      bool? deleted,
      string? search,
      EducationSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new GetEducationsQuery
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
    public async Task<ActionResult<EducationModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      EducationModel? model = await _pipeline.ExecuteAsync(new GetEducationQuery(id), cancellationToken);
      if (model == null)
      {
        return NotFound();
      }

      return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EducationModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateEducationPayload payload,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new UpdateEducationMutation(id, payload), cancellationToken));
    }
  }
}
