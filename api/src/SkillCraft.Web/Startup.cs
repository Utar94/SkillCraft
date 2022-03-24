using Logitar.WebApiToolKit;

namespace SkillCraft.Web
{
  public class Startup : StartupBase
  {
    private readonly ConfigurationOptions options = new();
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
      this.configuration = configuration;
    }

    public override void ConfigureServices(IServiceCollection services)
    {
      base.ConfigureServices(services);

      services.AddWebApiToolKit(configuration, options);
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
