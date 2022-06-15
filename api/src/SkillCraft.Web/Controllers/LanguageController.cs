using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Languages;
using SkillCraft.Core.Languages.Models;
using SkillCraft.Core.Languages.Mutations;
using SkillCraft.Core.Languages.Payloads;
using SkillCraft.Core.Languages.Queries;
using SkillCraft.Web.Filters;
using SkillCraft.Web.Pipeline;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("languages")]
  public class LanguageController : ControllerBase
  {
    private readonly IRequestPipeline _pipeline;

    public LanguageController(IRequestPipeline pipeline)
    {
      _pipeline = pipeline;
    }

    [HttpPost]
    public async Task<ActionResult<LanguageModel>> CreateAsync(
      [FromBody] CreateLanguagePayload payload,
      CancellationToken cancellationToken
    )
    {
      LanguageModel model = await _pipeline.ExecuteAsync(new CreateLanguageMutation(payload), cancellationToken);
      var uri = new Uri($"/languages/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<LanguageModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _pipeline.ExecuteAsync(new DeleteLanguageMutation(id), cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<LanguageModel>> GetAsync(
      bool? deleted,
      string? search,
      LanguageSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new GetLanguagesQuery
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
    public async Task<ActionResult<LanguageModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      LanguageModel? model = await _pipeline.ExecuteAsync(new GetLanguageQuery(id), cancellationToken);
      if (model == null)
      {
        return NotFound();
      }

      return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LanguageModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateLanguagePayload payload,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new UpdateLanguageMutation(id, payload), cancellationToken));
    }
  }
}
