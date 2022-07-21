using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Customizations;
using SkillCraft.Core.Customizations.Models;
using SkillCraft.Core.Customizations.Payload;

namespace SkillCraft.Web.Controllers
{
  /// <summary>
  /// TODO(fpion): Authorization with World
  /// </summary>
  [ApiController]
  [Route("customizations")]
  public class CustomizationController : ControllerBase
  {
    private readonly ICustomizationService _customizationService;

    public CustomizationController(ICustomizationService customizationService)
    {
      _customizationService = customizationService;
    }

    [HttpPost]
    public async Task<ActionResult<CustomizationModel>> CreateAsync([FromBody] CreateCustomizationPayload payload, CancellationToken cancellationToken)
    {
      CustomizationModel customization = await _customizationService.CreateAsync(payload, cancellationToken);
      var uri = new Uri($"/customizations/{customization.Id}", UriKind.Relative);

      return Created(uri, customization);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<CustomizationModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _customizationService.DeleteAsync(id, cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<CustomizationModel>> GetAsync(string? search, CustomizationType? type,
      CustomizationSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return Ok(await _customizationService.GetAsync(search, type, sort, desc, index, count, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomizationModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      CustomizationModel? customization = await _customizationService.GetAsync(id, cancellationToken);
      if (customization == null)
      {
        return NotFound();
      }

      return Ok(customization);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CustomizationModel>> UpdateAsync(Guid id, [FromBody] UpdateCustomizationPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _customizationService.UpdateAsync(id, payload, cancellationToken));
    }
  }
}
