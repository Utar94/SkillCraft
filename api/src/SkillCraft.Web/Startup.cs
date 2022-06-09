using Logitar.AspNetCore.Identity;
using Logitar.Identity.EntityFrameworkCore;
using Logitar.WebApiToolKit;
using SkillCraft.Infrastructure;

namespace SkillCraft.Web
{
  public class Startup : StartupBase
  {
    private readonly IConfiguration _configuration;
    private readonly ConfigurationOptions _options = new();

    public Startup(IConfiguration configuration) : base()
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

      services.AddSkillCraftInfrastructure();
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

        application.UseWebApiToolKit(_options);
      }
    }
  }
}
