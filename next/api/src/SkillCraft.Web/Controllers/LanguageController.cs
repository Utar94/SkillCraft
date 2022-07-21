using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Languages;
using SkillCraft.Core.Languages.Models;
using SkillCraft.Core.Languages.Payload;

namespace SkillCraft.Web.Controllers
{
  /// <summary>
  /// TODO(fpion): Authorization with World
  /// </summary>
  [ApiController]
  [Route("languages")]
  public class LanguageController : ControllerBase
  {
    private readonly ILanguageService _languageService;

    public LanguageController(ILanguageService languageService)
    {
      _languageService = languageService;
    }

    [HttpPost]
    public async Task<ActionResult<LanguageModel>> CreateAsync([FromBody] CreateLanguagePayload payload, CancellationToken cancellationToken)
    {
      LanguageModel language = await _languageService.CreateAsync(payload, cancellationToken);
      var uri = new Uri($"/languages/{language.Id}", UriKind.Relative);

      return Created(uri, language);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<LanguageModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _languageService.DeleteAsync(id, cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<LanguageModel>> GetAsync(bool? isExotic, string? search,
      LanguageSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return Ok(await _languageService.GetAsync(isExotic, search, sort, desc, index, count, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LanguageModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      LanguageModel? language = await _languageService.GetAsync(id, cancellationToken);
      if (language == null)
      {
        return NotFound();
      }

      return Ok(language);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LanguageModel>> UpdateAsync(Guid id, [FromBody] UpdateLanguagePayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _languageService.UpdateAsync(id, payload, cancellationToken));
    }
  }
}
