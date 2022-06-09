using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SkillCraft.Infrastructure
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddSkillCraftInfrastructure(this IServiceCollection services)
    {
      return services
        .AddDbContext<SkillCraftDbContext>((provider, builder) =>
        {
          var configuration = provider.GetRequiredService<IConfiguration>();
          builder.UseNpgsql(configuration.GetConnectionString(nameof(SkillCraftDbContext)));
        });
    }
  }
}
