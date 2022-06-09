using Logitar.WebApiToolKit.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace SkillCraft.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Route("")]
  public class IndexController : ControllerBase
  {
    private readonly ApiSettings _apiSettings;

    public IndexController(ApiSettings apiSettings)
    {
      _apiSettings = apiSettings;
    }

    [HttpGet]
    public IActionResult Get() => Ok(_apiSettings.Name);
  }
}
