using Microsoft.EntityFrameworkCore;
using SkillCraft.Infrastructure;
using SkillCraft.Web;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

WebApplication application = builder.Build();

startup.Configure(application);

if (application.Environment.IsDevelopment())
{
  using IServiceScope scope = application.Services.CreateScope();
  using var dbContext = scope.ServiceProvider.GetRequiredService<SkillCraftDbContext>();
  dbContext.Database.Migrate();
}

application.Run();
