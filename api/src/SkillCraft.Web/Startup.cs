using Logitar.AspNetCore.Identity;
using Logitar.Identity.EntityFrameworkCore;
using Logitar.WebApiToolKit;
using Logitar.WebApiToolKit.Configuration;
using Microsoft.AspNetCore.Cors.Infrastructure;
using SkillCraft.Core;
using SkillCraft.Infrastructure;
using SkillCraft.Web.Middlewares;
using SkillCraft.Web.Pipeline;

namespace SkillCraft.Web
{
  public class Startup : StartupBase
  {
    private readonly IConfiguration _configuration;
    private readonly ConfigurationOptions _options = new();

    public Startup(IConfiguration configuration)
    {
      _configuration = configuration;

      _options.Filters.Add<IdentityExceptionFilterAttribute>();
    }

    public override void ConfigureServices(IServiceCollection services)
    {
      base.ConfigureServices(services);

      services.AddWebApiToolKit(_configuration, _options);

      services.AddDefaultIdentity(_configuration)
        .WithEntityFrameworkStores<SkillCraftDbContext>();

      services.AddSkillCraftCore();
      services.AddSkillCraftInfrastructure();

      services.AddSingleton<IApplicationContext, HttpApplicationContext>();
      services.AddScoped<IRequestPipeline, HttpRequestPipeline>();
    }

    public override void Configure(IApplicationBuilder applicationBuilder)
    {
      if (applicationBuilder is WebApplication application)
      {
        if (application.Environment.IsDevelopment())
        {
          application.UseSwagger();
          application.UseSwaggerUI();
        }

        #region TODO(fpion): refactor
        // application.UseWebApiToolKit(_options);
        var apiSettings = application.Services.GetService<ApiSettings>();
        if (apiSettings != null && apiSettings.Environments?.Contains(application.Environment.EnvironmentName) != false)
        {
          application.UseSwagger();
          application.UseSwaggerUI(config => config.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            apiSettings.Name
          ));
        }

        if (_options.UseHttpsRedirection)
        {
          application.UseHttpsRedirection();
        }

        if (_options.UseRazorPages)
        {
          application.MapRazorPages();
        }

        var corsSettings = application.Services.GetService<CorsSettings>();
        if (corsSettings != null)
        {
          application.UseCors(builder => ConfigureCors(builder, corsSettings));
        }

        if (_options.UseAuthentication)
        {
          application.UseAuthentication();
          application.UseAuthorization();
        }

        application.UseMiddleware<LoggingMiddleware>();

        application.MapControllers();
        #endregion
      }
    }

    #region TODO(fpion): refactor
    private static void ConfigureCors(CorsPolicyBuilder builder, CorsSettings settings)
    {
      if (settings.AllowedOrigins == null)
      {
        builder.AllowAnyOrigin();
      }
      else
      {
        builder.WithOrigins(settings.AllowedOrigins);
      }

      if (settings.AllowedMethods == null)
      {
        builder.AllowAnyMethod();
      }
      else
      {
        builder.WithMethods(settings.AllowedMethods);
      }

      if (settings.AllowedHeaders == null)
      {
        builder.AllowAnyHeader();
      }
      else
      {
        builder.WithHeaders(settings.AllowedHeaders);
      }
    }
    #endregion
  }
}
