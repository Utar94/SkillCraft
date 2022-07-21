using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core;
using SkillCraft.Core.Educations;
using SkillCraft.Core.Educations.Models;
using SkillCraft.Core.Educations.Payload;

namespace SkillCraft.Web.Controllers
{
  /// <summary>
  /// TODO(fpion): Authorization with World
  /// </summary>
  [ApiController]
  [Route("educations")]
  public class EducationController : ControllerBase
  {
    private readonly IEducationService _educationService;

    public EducationController(IEducationService educationService)
    {
      _educationService = educationService;
    }

    [HttpPost]
    public async Task<ActionResult<EducationModel>> CreateAsync([FromBody] CreateEducationPayload payload, CancellationToken cancellationToken)
    {
      EducationModel education = await _educationService.CreateAsync(payload, cancellationToken);
      var uri = new Uri($"/educations/{education.Id}", UriKind.Relative);

      return Created(uri, education);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<EducationModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _educationService.DeleteAsync(id, cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<EducationModel>> GetAsync(string? search, Skill? skill,
      EducationSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return Ok(await _educationService.GetAsync(search, skill, sort, desc, index, count, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EducationModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      EducationModel? education = await _educationService.GetAsync(id, cancellationToken);
      if (education == null)
      {
        return NotFound();
      }

      return Ok(education);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EducationModel>> UpdateAsync(Guid id, [FromBody] UpdateEducationPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _educationService.UpdateAsync(id, payload, cancellationToken));
    }
  }
}
