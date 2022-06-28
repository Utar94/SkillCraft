using Microsoft.EntityFrameworkCore;
using SkillCraft.Infrastructure;

namespace SkillCraft.Web
{
  public class Program
  {
    public static void Main(string[] args)
    {
      WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

      var startup = new Startup(builder.Configuration);
      startup.ConfigureServices(builder.Services);

      WebApplication application = builder.Build();

      startup.Configure(application);

      if (builder.Configuration.GetValue<bool>("MigrateDatabase"))
      {
        using IServiceScope scope = application.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<SkillCraftDbContext>();
        dbContext.Database.Migrate();
      }

      application.Run();
    }
  }
}
