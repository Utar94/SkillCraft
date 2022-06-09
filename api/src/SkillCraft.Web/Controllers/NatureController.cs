using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Natures;
using SkillCraft.Core.Natures.Models;
using SkillCraft.Core.Natures.Mutations;
using SkillCraft.Core.Natures.Payloads;
using SkillCraft.Core.Natures.Queries;
using SkillCraft.Web.Filters;
using SkillCraft.Web.Pipeline;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("natures")]
  public class NatureController : ControllerBase
  {
    private readonly IRequestPipeline _pipeline;

    public NatureController(IRequestPipeline pipeline)
    {
      _pipeline = pipeline;
    }

    [HttpPost]
    public async Task<ActionResult<NatureModel>> CreateAsync(
      [FromBody] CreateNaturePayload payload,
      CancellationToken cancellationToken
    )
    {
      NatureModel model = await _pipeline.ExecuteAsync(new CreateNatureMutation(payload), cancellationToken);
      var uri = new Uri($"/natures/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<NatureModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _pipeline.ExecuteAsync(new DeleteNatureMutation(id), cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<NatureModel>> GetAsync(
      bool? deleted,
      string? search,
      NatureSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new GetNaturesQuery
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
    public async Task<ActionResult<NatureModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      NatureModel? model = await _pipeline.ExecuteAsync(new GetNatureQuery(id), cancellationToken);
      if (model == null)
      {
        return NotFound();
      }

      return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<NatureModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateNaturePayload payload,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new UpdateNatureMutation(id, payload), cancellationToken));
    }
  }
}
