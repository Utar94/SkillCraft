using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Castes;
using SkillCraft.Core.Castes.Models;
using SkillCraft.Core.Castes.Mutations;
using SkillCraft.Core.Castes.Payloads;
using SkillCraft.Core.Castes.Queries;
using SkillCraft.Web.Filters;
using SkillCraft.Web.Pipeline;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("castes")]
  public class CasteController : ControllerBase
  {
    private readonly IRequestPipeline _pipeline;

    public CasteController(IRequestPipeline pipeline)
    {
      _pipeline = pipeline;
    }

    [HttpPost]
    public async Task<ActionResult<CasteModel>> CreateAsync(
      [FromBody] CreateCastePayload payload,
      CancellationToken cancellationToken
    )
    {
      CasteModel model = await _pipeline.ExecuteAsync(new CreateCasteMutation(payload), cancellationToken);
      var uri = new Uri($"/castes/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<CasteModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _pipeline.ExecuteAsync(new DeleteCasteMutation(id), cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<CasteModel>> GetAsync(
      bool? deleted,
      string? search,
      CasteSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new GetCastesQuery
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
    public async Task<ActionResult<CasteModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      CasteModel? model = await _pipeline.ExecuteAsync(new GetCasteQuery(id), cancellationToken);
      if (model == null)
      {
        return NotFound();
      }

      return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CasteModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateCastePayload payload,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new UpdateCasteMutation(id, payload), cancellationToken));
    }
  }
}
