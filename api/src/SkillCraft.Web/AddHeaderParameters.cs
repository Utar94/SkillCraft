using Microsoft.OpenApi.Models;
using SkillCraft.Web.Middlewares;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SkillCraft.Web
{
  public class AddHeaderParameters : IOperationFilter
  {
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
      operation.Parameters.Add(new OpenApiParameter
      {
        Description = "Enter your world ID or alias in the input below.",
        In = ParameterLocation.Header,
        Name = WorldMiddleware.HeaderKey
      });
    }
  }
}
