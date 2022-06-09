using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Customizations;
using SkillCraft.Core.Customizations.Models;
using SkillCraft.Core.Customizations.Mutations;
using SkillCraft.Core.Customizations.Payloads;
using SkillCraft.Core.Customizations.Queries;
using SkillCraft.Web.Filters;
using SkillCraft.Web.Pipeline;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("customizations")]
  public class CustomizationController : ControllerBase
  {
    private readonly IRequestPipeline _pipeline;

    public CustomizationController(IRequestPipeline pipeline)
    {
      _pipeline = pipeline;
    }

    [HttpPost]
    public async Task<ActionResult<CustomizationModel>> CreateAsync(
      [FromBody] CreateCustomizationPayload payload,
      CancellationToken cancellationToken
    )
    {
      CustomizationModel model = await _pipeline.ExecuteAsync(new CreateCustomizationMutation(payload), cancellationToken);
      var uri = new Uri($"/customizations/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<CustomizationModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _pipeline.ExecuteAsync(new DeleteCustomizationMutation(id), cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<CustomizationModel>> GetAsync(
      bool? deleted,
      string? search,
      CustomizationType? type,
      CustomizationSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new GetCustomizationsQuery
      {
        Deleted = deleted,
        Search = search,
        Type = type,
        Sort = sort,
        Desc = desc,
        Index = index,
        Count = count
      }, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomizationModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      CustomizationModel? model = await _pipeline.ExecuteAsync(new GetCustomizationQuery(id), cancellationToken);
      if (model == null)
      {
        return NotFound();
      }

      return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CustomizationModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateCustomizationPayload payload,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new UpdateCustomizationMutation(id, payload), cancellationToken));
    }
  }
}
