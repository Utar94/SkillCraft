using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Core;

namespace SkillCraft.Infrastructure
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddSkillCraftInfrastructure(this IServiceCollection services)
    {
      return services
        .AddDbContext<IDbContext, SkillCraftDbContext>(ConfigureDbContext)
        .AddDbContext<SkillCraftDbContext>(ConfigureDbContext);
    }

    private static void ConfigureDbContext(IServiceProvider provider, DbContextOptionsBuilder builder)
    {
      var configuration = provider.GetRequiredService<IConfiguration>();
      builder.UseNpgsql(configuration.GetConnectionString(nameof(SkillCraftDbContext)));
    }
  }
}
