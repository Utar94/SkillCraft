using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SkillCraft.Web.Filters
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class RequireWorldAttribute : Attribute, IAuthorizationFilter
  {
    public void OnAuthorization(AuthorizationFilterContext context)
    {
      if (context.HttpContext.GetWorld() == null)
      {
        context.Result = new JsonResult(new { code = "missing_world" })
        {
          StatusCode = StatusCodes.Status400BadRequest
        };
      }
    }
  }
}
