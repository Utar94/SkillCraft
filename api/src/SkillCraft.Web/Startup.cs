using Logitar.AspNetCore.Identity;
using Logitar.Email.SendGrid;
using Logitar.Identity.EntityFrameworkCore;
using Logitar.Validation;
using Logitar.WebApiToolKit;
using Logitar.WebApiToolKit.Configuration;
using Microsoft.AspNetCore.Cors.Infrastructure;
using RazorLight;
using SkillCraft.Core;
using SkillCraft.Infrastructure;
using SkillCraft.Web.Email;
using SkillCraft.Web.Middlewares;
using SkillCraft.Web.Settings;
using System.Reflection;

namespace SkillCraft.Web
{
  public class Startup : StartupBase
  {
    private readonly ConfigurationOptions options = new();
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
      this.configuration = configuration;

      options.Filters.Add<IdentityExceptionFilterAttribute>();
    }

    public override void ConfigureServices(IServiceCollection services)
    {
      base.ConfigureServices(services);

      var applicationSettings = configuration.GetSection("Application").Get<ApplicationSettings>() ?? new();
      CompositeValidator.Validate(applicationSettings);
      services.AddSingleton(applicationSettings);

      services.AddApplicationInsightsTelemetry();

      services.AddWebApiToolKit(configuration, options);

      services.AddDefaultIdentity(configuration)
        .WithEntityFrameworkStores<SkillCraftDbContext>();

      services.AddSingleton<IRazorLightEngine>(_ => new RazorLightEngineBuilder()
        .SetOperatingAssembly(Assembly.GetExecutingAssembly())
        .UseFileSystemProject(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location))
        .UseMemoryCachingProvider()
        .Build()
      );

      services.AddCore();
      services.AddInfrastructure();
      services.AddSendGrid();

      services.AddSingleton<IEmailService, EmailService>();
      services.AddSingleton<IUserContext, HttpUserContext>();
    }

    public override void Configure(IApplicationBuilder applicationBuilder)
    {
      if (applicationBuilder is WebApplication application)
      {
        #region TODO(fpion): refactor
        var apiSettings = application.Services.GetService<ApiSettings>();
        if (apiSettings != null && apiSettings.Environments?.Contains(application.Environment.EnvironmentName) != false)
        {
          application.UseSwagger();
          application.UseSwaggerUI(config => config.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            apiSettings.Name
          ));
        }

        if (options.UseHttpsRedirection)
        {
          application.UseHttpsRedirection();
        }

        if (options.UseRazorPages)
        {
          application.MapRazorPages();
        }

        var corsSettings = application.Services.GetService<CorsSettings>();
        if (corsSettings != null)
        {
          application.UseCors(builder => ConfigureCors(builder, corsSettings));
        }

        if (options.UseAuthentication)
        {
          application.UseAuthentication();
          application.UseMiddleware<WorldMiddleware>();
          application.UseAuthorization();
        }

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
