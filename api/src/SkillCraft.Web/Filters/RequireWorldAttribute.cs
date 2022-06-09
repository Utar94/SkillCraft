using Microsoft.AspNetCore.Mvc.Filters;
using SkillCraft.Core.Logging;

namespace SkillCraft.Web.Filters
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class RequireWorldAttribute : Attribute, IAuthorizationFilter
  {
    public void OnAuthorization(AuthorizationFilterContext context)
    {
      if (context.HttpContext.GetWorld() == null)
      {
        throw Error.FailureException(ErrorCode.WorldRequired, "The world is required.");
      }
    }
  }
}
