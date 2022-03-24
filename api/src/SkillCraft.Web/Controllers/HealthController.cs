using Microsoft.AspNetCore.Mvc;

namespace SkillCraft.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Route("health")]
  public class HealthController : ControllerBase
  {
    [HttpGet]
    public IActionResult Get() => NoContent();
  }
}
