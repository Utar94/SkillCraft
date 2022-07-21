using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SkillCraft.Web.Filters
{
  internal class ValidationExceptionFilterAttribute : ExceptionFilterAttribute
  {
    public override void OnException(ExceptionContext context)
    {
      if (context.Exception is ValidationException exception)
      {
        context.ExceptionHandled = true;
        context.Result = new JsonResult(exception.Errors) // TODO(fpion): implement
        {
          StatusCode = StatusCodes.Status400BadRequest
        };
      }
    }
  }
}
