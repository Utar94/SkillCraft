using Microsoft.AspNetCore.Mvc;

namespace SkillCraft.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Route("")]
  public class IndexController : ControllerBase
  {
    [HttpGet]
    public IActionResult Get() => Ok("SkillCraft API v0");
  }
}
