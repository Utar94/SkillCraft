using Logitar.AspNetCore.Identity;
using Logitar.Email.SendGrid;
using Logitar.Identity.EntityFrameworkCore;
using Logitar.Validation;
using Logitar.WebApiToolKit;
using RazorLight;
using SkillCraft.Core;
using SkillCraft.Infrastructure;
using SkillCraft.Web.Email;
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
    }

    public override void Configure(IApplicationBuilder applicationBuilder)
    {
      if (applicationBuilder is WebApplication application)
      {
        application.UseWebApiToolKit(options);
      }
    }
  }
}
