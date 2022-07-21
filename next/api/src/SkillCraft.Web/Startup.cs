using SkillCraft.Core;
using SkillCraft.Infrastructure;
using SkillCraft.Web.Filters;
using System.Text.Json.Serialization;

namespace SkillCraft.Web
{
  internal class Startup : StartupBase
  {
    public override void ConfigureServices(IServiceCollection services)
    {
      base.ConfigureServices(services);

      services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
        .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
      services.AddCors();
      services.AddHttpContextAccessor();
      services.AddOpenApi();

      services.AddSkillCraftCore();
      services.AddSkillCraftInfrastructure();

      services.AddSingleton<IUserContext, HttpUserContext>();
    }

    public override void Configure(IApplicationBuilder applicationBuilder)
    {
      if (applicationBuilder is WebApplication application)
      {
        if (application.Environment.IsDevelopment())
        {
          application.UseSwagger();
          application.UseSwaggerUI(config => config.SwaggerEndpoint("/swagger/v1/swagger.json", "SkillCraft API v1.0"));
        }

        application.UseHttpsRedirection();
        application.UseCors();
        application.UseAuthentication();
        application.UseAuthorization();
        application.MapControllers();
      }
    }
  }
}
