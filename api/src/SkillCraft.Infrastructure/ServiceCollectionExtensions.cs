using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SkillCraft.Infrastructure
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
      return services
        .AddDbContext<SkillCraftDbContext>((provider, builder) =>
        {
          var configuration = provider.GetRequiredService<IConfiguration>();
          builder.UseNpgsql(configuration.GetValue<string>($"POSTGRESQLCONNSTR_{nameof(SkillCraftDbContext)}"));
        });
    }
  }
}
