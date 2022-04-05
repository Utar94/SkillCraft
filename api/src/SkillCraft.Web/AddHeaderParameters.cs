using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SkillCraft.Web
{
  public class AddHeaderParameters : IOperationFilter
  {
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
      operation.Parameters.Add(new OpenApiParameter
      {
        Description = "Enter your world ID in the input below.",
        In = ParameterLocation.Header,
        Name = "World"
      });
    }
  }
}
